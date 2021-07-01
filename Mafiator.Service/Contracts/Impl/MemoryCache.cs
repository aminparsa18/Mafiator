using System;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.Distributed;

namespace Mafiator.Service.Contracts.Impl
{
    public class MemoryCache : IMemoryCache
    {
        public IDistributedCache _cache;
        private readonly MessagePackSerializerOptions serializerSettings;

        public MemoryCache(IDistributedCache _cache)
        {
            serializerSettings =
                ContractlessStandardResolver.Options.WithCompression(MessagePackCompression.Lz4BlockArray);
            this._cache = _cache;
        }

        public void SetCache<T>(T values, string key)
        {
            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(6),
                SlidingExpiration = TimeSpan.FromMinutes(3),
            };
            _cache.Set(key, MessagePackSerializer.Serialize(values, serializerSettings), cacheOptions);
        }

        public T GetCache<T>(string key) where T : class
        {
                var values = _cache.Get(key);
                return values == null ? null : MessagePackSerializer.Deserialize<T>(values, serializerSettings);
        }

        public void RemoveCache(string key)
        {
                _cache.Remove(key);
        }
    }
}