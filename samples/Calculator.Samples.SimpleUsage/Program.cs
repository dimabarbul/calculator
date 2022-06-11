using Calculator.Core.DependencyInjection;
using Calculator.Extra.Operators;
using Calculator.Samples.ExtendingCalculator.Operators;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Samples.SimpleUsage;

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

            decimal result = calculator.Calculate<decimal>(formula);

            Console.WriteLine("{0} = {1}", formula, result);
        }
    }

    private static IServiceProvider SetupDI()
    {
        return new ServiceCollection()
            .AddCalculator(
                typeof(AddOperator).Assembly,
                typeof(LengthOperator).Assembly)
            .BuildServiceProvider(validateScopes: true);
    }
}