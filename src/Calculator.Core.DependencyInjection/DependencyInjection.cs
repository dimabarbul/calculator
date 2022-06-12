using System.Reflection;
using Calculator.Core.Parsers;
using Calculator.Core.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Core.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddCalculator(this IServiceCollection services, params Assembly[] extraAssemblies)
    {
        Type[] allTypes = GetAllTypes(extraAssemblies).ToArray();

        return services
            .AddParsers(allTypes)
            .AddOperations(allTypes)
            .AddScoped<Calculator>()
            .AddScoped<FormulaTokenizer>();
    }

    private static IServiceCollection AddParsers(this IServiceCollection services, Type[] types)
    {
        return services.AddImplementations<IParser>(types);
    }

    private static IServiceCollection AddOperations(this IServiceCollection services, Type[] types)
    {
        return services.AddImplementations<Operation>(types);
    }

    private static IServiceCollection AddImplementations<TServiceType>(this IServiceCollection services, Type[] types)
    {
        foreach (Type parserType in types
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