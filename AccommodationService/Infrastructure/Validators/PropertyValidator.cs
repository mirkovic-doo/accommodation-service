using AccommodationService.Controllers.Property.Requests;
using FluentValidation;

namespace AccommodationService.Infrastructure.Validators;

public class PropertyValidator : AbstractValidator<PropertyRequest>
{
    public PropertyValidator()
    {
        RuleFor(x => x.MinGuests).GreaterThan(0).NotEmpty();
        RuleFor(x => x.MaxGuests).GreaterThan(0).NotEmpty();
        RuleFor(x => x).Must(x => x.MinGuests <= x.MaxGuests).WithMessage("MinGuests must be less than or equal to MaxGuests.");
    }
}
