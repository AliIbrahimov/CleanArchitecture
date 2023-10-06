using Core.CrossCuttingConcerns.Exceptions.Types;
using FluentValidation;
using MediatR;
using ValidationException = Core.CrossCuttingConcerns.Exceptions.Types.ValidationException;

namespace Core.Application.Pipelines.Validation;

public class RequstValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequstValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationContext<object> context = new(request);

        IEnumerable<ValidationExceptionModel> errors = _validators
            .Select(validate=>validate.Validate(context)) 
            .SelectMany (validate=>validate.Errors)
            .Where(failure=>failure != null)    
            .GroupBy(
                keySelector:p=>p.PropertyName,
                resultSelector:(propertyName,errors)=>
                    new ValidationExceptionModel
                    {
                        Property = propertyName,
                        Errors = errors.Select(e => e.ErrorMessage)
                    })
                        .ToList();
        if (errors.Any())
            throw new ValidationException(errors);
        TResponse response = await next();
        return response;
    }
}
