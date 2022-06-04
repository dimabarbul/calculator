using System.Reflection;
using Calculator.Core.Operations;
using Calculator.Core.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Core.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddCalculator(this IServiceCollection services)
    {
        return services
            .AddParsers()
            .AddOperations()
            .AddScoped<Calculator>()
            .AddScoped<FormulaTokenizer>();
    }

    private static IServiceCollection AddParsers(this IServiceCollection services)
    {
        return services.AddImplementations<IParser>();
    }

    private static IServiceCollection AddOperations(this IServiceCollection services)
    {
        return services.AddImplementations<Operation>();
    }

    private static IServiceCollection AddImplementations<TServiceType>(this IServiceCollection services)
    {
        foreach (Type parserType in GetAllTypes()
                     .Where(t => t.IsAssignableTo(typeof(TServiceType)))
                     .Where(t => t.IsClass && !t.IsAbstract))
        {
            services.AddScoped(typeof(TServiceType), parserType);
        }

        return services;
    }

    private static IEnumerable<Type> GetAllTypes()
    {
        return Assembly.GetCallingAssembly()
            .GetReferencedAssemblies()
            .SelectMany(an => Assembly.Load(an).GetTypes());
    }
}