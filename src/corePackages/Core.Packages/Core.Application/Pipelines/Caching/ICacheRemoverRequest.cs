namespace Core.Application.Pipelines.Caching;

public interface ICacheRemoverRequest
{
    string CashKey { get; }
    bool BypassCache { get; }
}
