using Mafiator.Repository.Cache;
using RepoDb;
using RepoDb.Interfaces;
using System;
using System.Collections;

namespace Mafiator.Repository
{
    public class FastCache:ICache
    {

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Add<T>(string key, T value, int expiration = 180, bool throwException = true)
        {
             Barrel.Current.Add(key,value,TimeSpan.FromMinutes(expiration));
        }

        public void Add<T>(CacheItem<T> item, bool throwException = true)
        {
            Barrel.Current.Add(item.Key,item.Value,(DateTime.Now- item.Expiration));

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
    }
}
