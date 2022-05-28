using System.Reflection;
using Calculator.Core.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Core.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddCalculator(this IServiceCollection services)
    {
        Assembly? entryAssembly = Assembly.GetEntryAssembly();

        if (entryAssembly != null)
        {
            foreach (Type parserType in entryAssembly.GetReferencedAssemblies()
                .SelectMany(an => Assembly.Load(an).GetTypes())
                .Where(t => t.IsAssignableTo(typeof(IParser))))
            {
                services.AddScoped(typeof(IParser), parserType);
            }
        }

        return services;
    }
}