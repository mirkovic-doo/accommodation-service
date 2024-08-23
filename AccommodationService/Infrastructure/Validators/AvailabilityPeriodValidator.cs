using AccommodationService.Controllers.AvailabilityPeriod.Requests;
using FluentValidation;

namespace AccommodationService.Infrastructure.Validators;

public class AvailabilityPeriodValidator : AbstractValidator<AvailabilityPeriodRequest>
{
    public AvailabilityPeriodValidator()
    {
        RuleFor(x => x.PricePerDay).GreaterThan(0).NotEmpty();
        RuleFor(x => x).Must(x => x.StartDate <= x.EndDate).WithMessage("StartDate must be before or at the same date as EndDate.");
    }
}
