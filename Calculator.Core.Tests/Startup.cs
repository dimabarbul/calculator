using Calculator.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Calculator.Core.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCalculator();
    }
}