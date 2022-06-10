using Calculator.Core.DependencyInjection;
using Calculator.Extra.Operators;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.ConsoleApp;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 1 && (args[0] == "-h" || args[0] == "--help"))
        {
            Console.WriteLine(@"Usage: {0} [formula]", AppDomain.CurrentDomain.FriendlyName);
            return;
        }

        IServiceProvider servicesProvider = SetupDI();

        using IServiceScope serviceScope = servicesProvider.CreateScope();

        Core.Calculator calculator = serviceScope.ServiceProvider.GetRequiredService<Core.Calculator>();

        if (args.Length == 1)
        {
            Console.Write(calculator.Calculate<decimal>(args[0]));
        }
        else
        {
            Console.Write("Enter formula: ");
            string? formula = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(formula))
            {
                return;
            }

            long usedBytes = GC.GetTotalAllocatedBytes(true);
            decimal result = calculator.Calculate<decimal>(formula);
            usedBytes = GC.GetTotalAllocatedBytes(true) - usedBytes;

            Console.WriteLine("{0} = {1}", formula, result);

            Console.WriteLine($"Allocated (1st run): {usedBytes} bytes");

            usedBytes = GC.GetTotalAllocatedBytes(true);
            calculator.Calculate<decimal>(formula);
            usedBytes = GC.GetTotalAllocatedBytes(true) - usedBytes;
            Console.WriteLine($"Allocated (2nd run): {usedBytes} bytes");
        }
    }

    private static IServiceProvider SetupDI()
    {
        return new ServiceCollection()
            .AddCalculator(typeof(AddOperator).Assembly)
            .BuildServiceProvider(validateScopes: true);
    }
}