using Mafiator.Repository.Cache;
using RepoDb.Interfaces;
using System;
using System.Collections;
using System.Threading;

namespace Mafiator.Repository;

public class FastCache : ICache
{
    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void Add<T>(string key, T value, int expiration = 180, bool throwException = true)
    {
        Barrel.Current.Add(key, value, TimeSpan.FromMinutes(expiration));
    }

    public void Add<T>(CacheItem<T> item, bool throwException = true)
    {
        Barrel.Current.Add(item.Key, item.Value, (DateTime.Now - item.Expiration));

    }

    public void Clear()
    {
        Barrel.Current.EmptyAll();
    }

    public bool Contains(string key)
    {
        return Barrel.Current.Exists(key);
    }

    public CacheItem<T> Get<T>(string key, bool throwException = true)
    {
        return new CacheItem<T>(key, Barrel.Current.Get<T>(key));
    }

    public void Remove(string key, bool throwException = true)
    {
        Barrel.Current.Empty(key);
    }

    public Task AddAsync<T>(string key, T value, int expiration = 180, bool throwException = true, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task AddAsync<T>(CacheItem<T> item, bool throwException = true, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task ClearAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task<bool> ContainsAsync(string key, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task<CacheItem<T>> GetAsync<T>(string key, bool throwException = true, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public Task RemoveAsync(string key, bool throwException = true, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}