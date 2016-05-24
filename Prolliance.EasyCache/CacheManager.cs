using Amuse;

namespace Prolliance.EasyCache
{
    public class CacheManager
    {
        private static readonly string CACHE_CONFIG_NAME = "easy-cache";

        public static ICache Create()
        {
            ICache cache = Container.Create().Get<ICache>(CACHE_CONFIG_NAME);
            return cache;
        }
    }
}
