using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.IO;


namespace POSSystem.UI.Service
{
    public class CacheService : ICacheService
    {
        private ObjectCache cache;
        private CacheItemPolicy policy;

        public CacheService()
        {
            cache = MemoryCache.Default;

        }

        public void SetPolicy()
        {
            policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddDays(5);
        }

        public TValue ReadCache<TValue>(string cacheKey)
        {
            TValue value = (TValue)cache[cacheKey];
            return value;
        }

        public void SetCache<TValue>(string cacheKey, TValue value)
        {
            TValue val = ReadCache<TValue>(cacheKey);
            if(val != null)
            {
                cache.Remove(cacheKey);
            }
            cache.Set(cacheKey, value, policy);
        }

        public void RemoveCache(string cacheKey)
        {
            cache.Remove(cacheKey);
        }
    }
}
