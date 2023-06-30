using System.Security.Cryptography.X509Certificates;

namespace RedisCacheService
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        bool SetData<T>(string Key, T Value, DateTimeOffset expirationTime);
        object RemoveData(string key);

    }
}