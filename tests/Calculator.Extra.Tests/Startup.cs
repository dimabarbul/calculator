using System.Reflection;
using Calculator.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Extra.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCalculator(Assembly.Load("Calculator.Extra"));
    }
}