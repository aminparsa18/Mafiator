using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace Mafiator.Service.Contracts.Impl
{
    public class MemoryCache : IMemoryCache
    {
        public IDistributedCache Cache;
        private readonly MessagePackSerializerOptions _serializerSettings;

        public MemoryCache(IDistributedCache cache)
        {
            _serializerSettings =
                ContractlessStandardResolver.Options.WithCompression(MessagePackCompression.Lz4BlockArray);
            this.Cache = cache;
        }

        public void SetCache<T>(T values, string key)
        {
            var cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(6),
                SlidingExpiration = TimeSpan.FromMinutes(3),
            };
            Cache.Set(key, MessagePackSerializer.Serialize(values, _serializerSettings), cacheOptions);
        }

        public T GetCache<T>(string key) where T : class
        {
                var values = Cache.Get(key);
                return values == null ? null : MessagePackSerializer.Deserialize<T>(values, _serializerSettings);
        }

        public void RemoveCache(string key)
        {
                Cache.Remove(key);
        }
    }
}