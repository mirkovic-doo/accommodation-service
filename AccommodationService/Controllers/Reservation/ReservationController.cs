using AccommodationService.Application.Services;
using AccommodationService.Authorization;
using AccommodationService.Controllers.Reservation.Requests;
using AccommodationService.Controllers.Reservation.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationEntity = AccommodationService.Domain.Reservation;

namespace AccommodationService.Controllers.Reservation;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
public class ReservationController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IReservationService reservationService;

    public ReservationController(
        IMapper mapper,
        IReservationService reservationService)
    {
        this.mapper = mapper;
        this.reservationService = reservationService;
    }

    [HttpGet("{id}", Name = nameof(GetReservation))]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReservation([FromRoute] Guid id)
    {
        var property = await reservationService.GetAsync(id);

        return Ok(mapper.Map<ReservationResponse>(property));
    }

    [Authorize(nameof(AuthorizationLevel.Guest))]
    [HttpPost(Name = nameof(CreateReservation))]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateReservation([FromBody] ReservationRequest request)
    {
        var property = await reservationService.CreateAsync(mapper.Map<ReservationEntity>(request));

        return CreatedAtAction(nameof(CreateReservation), mapper.Map<ReservationResponse>(property));
    }

    [Authorize(nameof(AuthorizationLevel.Guest))]
    [HttpDelete("{id}", Name = nameof(DeleteReservation))]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteReservation([FromRoute] Guid id)
    {
        await reservationService.DeleteAsync(id);

        return Ok("Reservation deleted successfully");
    }

    [HttpGet("property/{propertyId}", Name = nameof(GetReservationsByPropertyId))]
    [ProducesResponseType(typeof(IEnumerable<ReservationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReservationsByPropertyId([FromRoute] Guid propertyId)
    {
        var reservations = await reservationService.GetAllByPropertyIdAsync(propertyId);

        return Ok(mapper.Map<IEnumerable<ReservationResponse>>(reservations));
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpGet("confirm/{id}", Name = nameof(ConfirmReservation))]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ConfirmReservation([FromRoute] Guid id)
    {
        await reservationService.ConfirmReservationAsync(id);

        return Ok("Reservation confirmed successfully");
    }

    [Authorize(nameof(AuthorizationLevel.Guest))]
    [HttpDelete("cancel/guest", Name = nameof(CancelReservationGuest))]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelReservationGuest([FromQuery] Guid id)
    {
        await reservationService.CancelReservationGuestAsync(id);

        return Ok("Reservation canceled successfully");
    }

    [Authorize(nameof(AuthorizationLevel.Guest))]
    [HttpDelete("cancel/host", Name = nameof(CancelReservationHost))]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelReservationHost([FromQuery] Guid id)
    {
        await reservationService.CancelReservationHostAsync(id);

        return Ok("Reservation canceled successfully");
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpGet("cancellednum/guest/{guestId}", Name = nameof(GetNumberOfCancelledReservations))]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNumberOfCancelledReservations([FromRoute] Guid guestId)
    {
        var num = await reservationService.GetNumberOfCancelledReservationsAsync(guestId);

        return Ok(new { canceledNum = num });
    }
}
