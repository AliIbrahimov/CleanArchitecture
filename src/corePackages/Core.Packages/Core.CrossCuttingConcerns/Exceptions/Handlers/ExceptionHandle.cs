using Core.CrossCuttingConcerns.Exceptions.Types;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers;

public abstract class ExceptionHandle
{
    public Task HandlerExceptionAsync(Exception exception) =>
        exception switch
        {
            BusinessException businessException => HandlerException(businessException),
            ValidationException validationException => HandlerException(validationException),
            _ => HandlerException(exception)
        };
    protected abstract Task HandlerException(BusinessException businessException);
    protected abstract Task HandlerException(ValidationException validationException);
    protected abstract Task HandlerException(Exception exception);

}
