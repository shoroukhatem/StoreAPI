using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service.Services.CacheService;
using System.Text;

namespace Store.API.Helper
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveInSeconds;

        public CacheAttribute(int timeToLiveInSeconds)
        {
            this.timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetCacheResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executeContext = await next();
            if (executeContext.Result is OkObjectResult response)
            {
                await cacheService.SetCacheResponseAsync(cacheKey, response.Value, TimeSpan.FromSeconds(timeToLiveInSeconds));
            }
        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {

            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                cacheKey.Append($"|{key}-{value}");
            }
            return cacheKey.ToString();
        }
    }
}
