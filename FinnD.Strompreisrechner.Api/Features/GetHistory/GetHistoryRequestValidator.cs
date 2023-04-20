using FluentValidation;

namespace FinnD.Strompreisrechner.Api.Features.GetHistory;

public sealed class GetHistoryRequestValidator : AbstractValidator<GetHistoryRequest>
{
    public GetHistoryRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The page number must be greater than or equal to 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("The page size must be greater than or equal to 1")
            .LessThanOrEqualTo(100)
            .WithMessage("The page size must be less than or equal to 100");
    }
}
