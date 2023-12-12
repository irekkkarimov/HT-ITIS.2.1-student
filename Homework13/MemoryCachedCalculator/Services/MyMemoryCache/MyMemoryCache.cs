using System.Diagnostics.CodeAnalysis;

namespace MemoryCachedCalculator.Services.MyMemoryCache;

[ExcludeFromCodeCoverage]
public class MyMemoryCache : IMyMemoryCache
{
    private readonly Dictionary<string, double> _cache = new();

    public (bool, double) Find(string key)
    {
        if (!_cache.ContainsKey(key))
            return (false, 0);

        return (true, _cache[key]);
    }

    public void Insert(string key, double value)
    {
        if (_cache.ContainsKey(key))
            return;

        _cache.Add(key, value);
    }
}