using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Application.Pipelines.Transaction;

public class TransactionScoperBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactionRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
        TResponse response;
		try
		{
			response = await next();
			scope.Complete();

		}
		catch (Exception)
		{
			scope.Dispose();
			throw;
		}
		return response;	
    }
}
