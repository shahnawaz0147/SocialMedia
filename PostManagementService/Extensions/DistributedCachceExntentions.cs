using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace PostManagementService.Extensions
{
    public static class DistributedCachceExntentions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,string recordID, T record,TimeSpan? expiryTime=null,TimeSpan? unsedExpireTime=null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = expiryTime ?? TimeSpan.FromHours(2);
            options.SlidingExpiration = unsedExpireTime;
            var jsondata = JsonSerializer.Serialize(record);
            await cache.SetStringAsync(recordID, jsondata, options);
        }
        public static async Task AddItemsinListCache<T>(this IDistributedCache cache, string recordID, T record, TimeSpan? expiryTime = null, TimeSpan? unsedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = expiryTime ?? TimeSpan.FromHours(2);
            options.SlidingExpiration = unsedExpireTime;
            List<T> existingList = new List<T>();
            var jsonData = await cache.GetStringAsync(recordID);
            if (jsonData != null)
            {
                existingList = JsonSerializer.Deserialize<List<T>>(jsonData);
            }
            if (existingList is null)
            {
                existingList = new List<T>();
            }
            existingList.Add(record);
            jsonData = JsonSerializer.Serialize(existingList);
            await cache.SetStringAsync(recordID, jsonData, options);
        }
        public static async Task<T> GetRecordAsyng<T>(this IDistributedCache cache, string recordID)
        {
            var jsonData = await cache.GetStringAsync(recordID);
            if(jsonData is null)
            {

                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }
        public static async Task DeleteFromListAsyn<T>(this IDistributedCache cache, string recordID, T record,Predicate<T> match, TimeSpan? expiryTime = null, TimeSpan? unsedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = expiryTime ?? TimeSpan.FromHours(2);
            options.SlidingExpiration = unsedExpireTime;
            List<T> existingList = new List<T>();
            var jsonData = await cache.GetStringAsync(recordID);
            if (jsonData != null)
            {
                existingList = JsonSerializer.Deserialize<List<T>>(jsonData);
            }
            if (existingList is null)
            {
                existingList = new List<T>();
            }
            existingList.RemoveAll(match);
            jsonData = JsonSerializer.Serialize(existingList);
            await cache.SetStringAsync(recordID, jsonData, options);
        }
    }
}
