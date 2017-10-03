using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace TravelTracker
{
    public class MemoryDbContextStartUp : Startup
    {
        public MemoryDbContextStartUp(IHostingEnvironment env) : base(env)
        {
        }

        protected sealed override void SetDbContext(DbContextOptionsBuilder options)
        {
            options.UseMemoryCache(new MemoryCache());
        }
    }

    public class MemoryCache : IMemoryCache
    {
        readonly Dictionary<object, CacheEntry> _dictionary = new Dictionary<object, CacheEntry>();

        public ICacheEntry CreateEntry(object key)
        {
            var cachEntry = new CacheEntry(key);
            _dictionary.Add(key, cachEntry);
            return cachEntry;
        }

        public void Dispose()
        {
            //Nothing to dispose
        }

        public void Remove(object key)
        {
            _dictionary.Remove(key);
        }

        public bool TryGetValue(object key, out object value)
        {
            CacheEntry cacheEntry;
            var isSuccessful = _dictionary.TryGetValue(key, out cacheEntry);

            value = cacheEntry.Value;
            return isSuccessful;
        }
    }

    public class CacheEntry : ICacheEntry
    {
        public CacheEntry(object key)
        {
            Key = key;
        }

        public object Key { get; private set; }

        public object Value { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }

        public IList<IChangeToken> ExpirationTokens => throw new NotImplementedException();

        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks => throw new NotImplementedException();

        public CacheItemPriority Priority { get; set; }

        public void Dispose()
        {
            //Nothing to dispose
        }
    }
}
