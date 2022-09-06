using RepoDb.Interfaces;

namespace Mafiator.Repository;

public static class CacheFactory
{
    private static readonly object SyncLock;
    private static ICache _cache;

    static CacheFactory()
    {
        SyncLock = new object();
    }

    public static ICache GetCache()
    {
        if (_cache != null) return _cache;
        lock (SyncLock)
        {
            _cache ??= new MemoryCache();
        }
        return _cache;
    }
}