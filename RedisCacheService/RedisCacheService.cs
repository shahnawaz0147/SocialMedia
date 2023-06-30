using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisCacheService
{
    public class RedisCacheService : ICacheService
    {
        IDatabase _cacheDb;

        public T GetData<T>(string key)
        {
            throw new NotImplementedException();
        }

        public object RemoveData(string key)
        {
            throw new NotImplementedException();
        }

        public bool SetData<T>(string Key, T Value, DateTimeOffset expirationTime)
        {
            throw new NotImplementedException();
        }
    }
}
