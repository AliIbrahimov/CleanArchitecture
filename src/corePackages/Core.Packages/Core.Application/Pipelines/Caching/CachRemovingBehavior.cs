using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Core.Application.Pipelines.Caching;

public class CachRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICacheRemoverRequest
{
    private readonly IDistributedCache _distributedCache;

    public CachRemovingBehavior(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next();
        TResponse response = await next();
        if (request.CashKey is not  null) 
            await _distributedCache.RemoveAsync(request.CashKey, cancellationToken);
        return response;
    }
}
