using System.Reflection;
using Calculator.Core.Parsers;
using Calculator.Core.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Core.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddCalculator(this IServiceCollection services, params Assembly[] extraAssemblies)
    {
        return services
            .AddParsers(extraAssemblies)
            .AddOperations(extraAssemblies)
            .AddScoped<Calculator>()
            .AddScoped<FormulaTokenizer>();
    }

    private static IServiceCollection AddParsers(this IServiceCollection services, Assembly[] extraAssemblies)
    {
        return services.AddImplementations<IParser>(extraAssemblies);
    }

    private static IServiceCollection AddOperations(this IServiceCollection services, Assembly[] extraAssemblies)
    {
        return services.AddImplementations<Operation>(extraAssemblies);
    }

    private static IServiceCollection AddImplementations<TServiceType>(this IServiceCollection services,
        Assembly[] extraAssemblies)
    {
        foreach (Type parserType in GetAllTypes(extraAssemblies)
            .Where(t => t.IsAssignableTo(typeof(TServiceType)))
            .Where(t => t.IsClass && !t.IsAbstract))
        {
            services.AddScoped(typeof(TServiceType), parserType);
        }

        return services;
    }

    private static IEnumerable<Type> GetAllTypes(Assembly[] extraAssemblies)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .Concat(extraAssemblies)
            .SelectMany(a => a.GetTypes());
    }
}