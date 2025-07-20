using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TicTacToe.Api.Filters;

[AttributeUsage(AttributeTargets.Method)]
internal sealed class IdempotentAttribute(
    int cacheTimeInMinutes = IdempotentAttribute.DefaultCacheTimeInMinutes)
    : Attribute, IAsyncActionFilter
{
    private const int DefaultCacheTimeInMinutes = 1;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(cacheTimeInMinutes);

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(
                "X-Payout-Idempotency",
                out var idempotenceKeyValue) || 
            !Guid.TryParse(idempotenceKeyValue, out var idempotenceKey))
        {
            context.Result = new BadRequestObjectResult("Invalid or missing Idempotence-Key header");
            return;
        }

        var cache = context.HttpContext
            .RequestServices.GetRequiredService<IDistributedCache>();

        var cacheKey = $"Idempotent_{idempotenceKey}";
        var cachedResult = await cache.GetStringAsync(cacheKey);
        if (cachedResult is not null)
        {
            var response = JsonSerializer.Deserialize<IdempotentResponse>(cachedResult);

            var result = new ObjectResult(response?.Value) { StatusCode = response?.StatusCode };
            context.Result = result;

            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult { StatusCode: >= 200 and < 300 } objectResult)
        {
            var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
            var response = new IdempotentResponse(statusCode, objectResult.Value);

            await cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(response),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheDuration }
            );
        }
    }
}
