using AccommodationService.Controllers.Reservation.Requests;
using FluentValidation;

namespace AccommodationService.Infrastructure.Validators;

public class ReservationValidator : AbstractValidator<ReservationRequest>
{
    public ReservationValidator()
    {
        RuleFor(x => x.Guests).GreaterThan(0).NotEmpty();

        RuleFor(x => x.StartDate).GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow)).NotEmpty();

        RuleFor(x => x.EndDate).GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow)).NotEmpty();
    }
}
