using AccommodationService.Domain;
using FluentValidation;

namespace AccommodationService.Infrastructure.Validators;

public class ReservationValidator : AbstractValidator<Reservation>
{
    public ReservationValidator()
    {
        RuleFor(x => x.Guests).GreaterThan(0).NotEmpty();
    }
}
