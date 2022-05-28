using System.Reflection;
using Calculator.Core.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Core.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddCalculator(this IServiceCollection services)
    {
        return AddParsers(services)
            .AddScoped<Calculator>()
            .AddScoped<FormulaTokenizer>()
            .AddScoped<OperationFactory>();
    }

    private static IServiceCollection AddParsers(IServiceCollection services)
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly != null)
        {
            foreach (Type parserType in entryAssembly.GetReferencedAssemblies()
                         .SelectMany(an => Assembly.Load(an).GetTypes())
                         .Where(t => t.IsAssignableTo(typeof(IParser)))
                         .Where(t => t.IsClass && !t.IsAbstract))
            {
                services.AddScoped(typeof(IParser), parserType);
            }
        }

        return services;
    }
}