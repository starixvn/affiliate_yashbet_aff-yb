namespace Fanex.Affiliate.User.Core.CoreService
{
    public interface IMemoryCacheService
    {
        bool Contains(string key);

        void Delete(string key);

        T Get<T>(string key);

        void Set<T>(string key, T value, int seconds = 120);
    }
}
