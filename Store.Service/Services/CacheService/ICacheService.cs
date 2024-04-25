using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.CacheService
{
    public interface ICacheService
    {
        //Add to cache memory
        Task SetCacheResponseAsync(string key,object response,TimeSpan timeToLive);
        //get from cache memory
        Task<string> GetCacheResponseAsync(string key);
    }
}
