using RepoDb;
using RepoDb.Interfaces;

namespace Mafiator.Repository
{
    public static class CacheFactory
    {
        private static readonly object syncLock;
        private static ICache cache = null;

        static CacheFactory()
        {
            syncLock = new object();
        }

        public static ICache GetCache()
        {
            if (cache != null) return cache;
            lock (syncLock)
            {
                cache ??= new MemoryCache();
            }
            return cache;
        }
    }
}
