namespace POSSystem.UI.Service
{
    public interface ICacheService
    {
        TValue ReadCache<TValue>(string cacheKey);
        void RemoveCache(string cacheKey);
        void SetCache<TValue>(string cacheKey, TValue value);
        void SetPolicy();
    }
}