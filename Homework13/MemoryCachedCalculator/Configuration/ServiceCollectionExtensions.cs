using System.Diagnostics.CodeAnalysis;
using MemoryCachedCalculator.Services;
using MemoryCachedCalculator.Services.CachedCalculator;
using MemoryCachedCalculator.Services.MathCalculator;
using MemoryCachedCalculator.Services.MyMemoryCache;

namespace MemoryCachedCalculator.Configuration;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMathCalculator(this IServiceCollection services)
    {
        return services.AddTransient<MathCalculatorService>();
    }

    public static IServiceCollection AddCachedMathCalculator(this IServiceCollection services)
    {
        return services.AddScoped<IMathCalculatorService>(s =>
            new MathCachedCalculatorService(
                s.GetRequiredService<IMyMemoryCache>(),
                s.GetRequiredService<MathCalculatorService>()));
    }
}