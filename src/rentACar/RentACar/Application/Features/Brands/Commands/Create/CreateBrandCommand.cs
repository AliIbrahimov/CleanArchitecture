using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Transaction;
using MediatR;

namespace Application.Features.Brands.Commands.Create;

public partial class CreateBrandCommand:IRequest<CreatedBrandResponse>,ITransactionRequest, ICacheRemoverRequest
{
    public string Name { get; set; }

    public string CashKey => throw new NotImplementedException();

    public bool BypassCache => throw new NotImplementedException();
}
