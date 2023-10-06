using FluentValidation;

namespace Application.Features.Brands.Commands.Create;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand> 
{
	public CreateBrandCommandValidator()
	{
		RuleFor(n=>n.Name).NotEmpty().MinimumLength(2);
	}
}
