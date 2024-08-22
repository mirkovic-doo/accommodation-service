using AccommodationService.Application.Services;
using AccommodationService.Authorization;
using AccommodationService.Controllers.AvailabilityPeriod.Requests;
using AccommodationService.Controllers.AvailabilityPeriod.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AvailabilityPeriodEntity = AccommodationService.Domain.AvailabilityPeriod;

namespace AccommodationService.Controllers.AvailabilityPeriod;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
public class AvailabilityPeriodController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAvailabilityPeriodService availabilityPeriodService;

    public AvailabilityPeriodController(
        IMapper mapper,
        IAvailabilityPeriodService availabilityPeriodService)
    {
        this.mapper = mapper;
        this.availabilityPeriodService = availabilityPeriodService;
    }

    [HttpGet("{id}", Name = nameof(GetAvailabilityPeriod))]
    [ProducesResponseType(typeof(AvailabilityPeriodResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailabilityPeriod([FromRoute] Guid id)
    {
        var availabilityPeriod = await availabilityPeriodService.GetAsync(id);

        return Ok(mapper.Map<AvailabilityPeriodResponse>(availabilityPeriod));
    }

    [HttpGet("property/{propertyId}", Name = nameof(GetAllByPropertyId))]
    [ProducesResponseType(typeof(IEnumerable<AvailabilityPeriodResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByPropertyId([FromRoute] Guid propertyId)
    {
        var availabilityPeriods = await availabilityPeriodService.GetAllByPropertyIdAsync(propertyId);

        return Ok(mapper.Map<IEnumerable<AvailabilityPeriodResponse>>(availabilityPeriods));
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpPost(Name = nameof(CreateAvailabilityPeriod))]
    [ProducesResponseType(typeof(AvailabilityPeriodResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAvailabilityPeriod([FromBody] AvailabilityPeriodRequest request)
    {
        var availabilityPeriod = await availabilityPeriodService.CreateAsync(mapper.Map<AvailabilityPeriodEntity>(request));

        return CreatedAtAction(nameof(CreateAvailabilityPeriod), mapper.Map<AvailabilityPeriodResponse>(availabilityPeriod));
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpPut(Name = nameof(UpdateAvailabilityPeriod))]
    [ProducesResponseType(typeof(AvailabilityPeriodResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAvailabilityPeriod([FromBody] AvailabilityPeriodRequest request)
    {
        var availabilityPeriod = await availabilityPeriodService.UpdateAsync(mapper.Map<AvailabilityPeriodEntity>(request));

        return Ok(mapper.Map<AvailabilityPeriodResponse>(availabilityPeriod));
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpDelete("{id}", Name = nameof(DeleteAvailabilityPeriod))]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteAvailabilityPeriod([FromRoute] Guid id)
    {
        await availabilityPeriodService.DeleteAsync(id);

        return Ok("Availability period deleted successfully");
    }
}
