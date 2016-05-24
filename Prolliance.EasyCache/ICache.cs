
namespace Prolliance.EasyCache
{
    public interface ICache
    {
        void Add(string key, object obj);
        object Get(string key);
        T Get<T>(string key);
        object Remove(string key);
        T Remove<T>(string key);
        void RemoveAll();
    }
}
