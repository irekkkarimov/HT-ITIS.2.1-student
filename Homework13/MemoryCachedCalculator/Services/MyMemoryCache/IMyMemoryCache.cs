namespace MemoryCachedCalculator.Services.MyMemoryCache;

public interface IMyMemoryCache
{
    public (bool, double) Find(string key);
    public void Insert(string key, double value);
}