using System.Collections.Generic;

namespace Prolliance.EasyCache.Impls
{
    public class MemoryCache : ICache
    {
        private static Dictionary<string, object> _Cache = new Dictionary<string, object>();

        public void Add(string key, object obj)
        {
            lock (_Cache)
            {
                _Cache[key] = obj;
            }
        }
        public object Get(string key)
        {
            try
            {
                if (_Cache.ContainsKey(key))
                {
                    return _Cache[key];
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public T Get<T>(string key)
        {
            return (T)this.Get(key);
        }
        public object Remove(string key)
        {
            lock (_Cache)
            {
                object rs = null;
                if (_Cache.ContainsKey(key))
                {
                    rs = _Cache[key];
                    _Cache.Remove(key);
                }
                return rs;
            }
        }
        public T Remove<T>(string key)
        {
            return (T)this.Remove(key);
        }

        public void RemoveAll()
        {
            _Cache.Clear();
        }
    }
}
