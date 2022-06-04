using Calculator.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.ConsoleApp;

public class Program
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
            System.Console.Write("Enter formula: ");
            string? formula = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(formula))
            {
                System.Console.WriteLine("{0} = {1}", formula, calculator.Calculate(formula));
            }
        }
    }

    private static IServiceProvider SetupDI()
    {
        return new ServiceCollection()
            .AddCalculator()
            .BuildServiceProvider(validateScopes: true);
    }
}