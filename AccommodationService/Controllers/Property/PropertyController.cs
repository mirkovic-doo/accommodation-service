using AccommodationService.Application.Services;
using AccommodationService.Authorization;
using AccommodationService.Controllers.Property.Requests;
using AccommodationService.Controllers.Property.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyEntity = AccommodationService.Domain.Property;

namespace AccommodationService.Controllers.Property;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
public class PropertyController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IPropertyService propertyService;

    public PropertyController(
        IMapper mapper,
        IPropertyService propertyService)
    {
        this.mapper = mapper;
        this.propertyService = propertyService;
    }

    [AllowAnonymous]
    [HttpGet("{id}", Name = nameof(GetProperty))]
    [ProducesResponseType(typeof(PropertyResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProperty([FromRoute] Guid id)
    {
        var property = await propertyService.GetAsync(id);

        return Ok(mapper.Map<PropertyResponse>(property));
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpPost(Name = nameof(CreateProperty))]
    [ProducesResponseType(typeof(PropertyResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProperty([FromBody] PropertyRequest request)
    {
        var property = await propertyService.CreateAsync(mapper.Map<PropertyEntity>(request));

        return CreatedAtAction(nameof(CreateProperty), mapper.Map<PropertyResponse>(property));
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpPut(Name = nameof(UpdateProperty))]
    [ProducesResponseType(typeof(PropertyResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProperty([FromBody] PropertyRequest request)
    {
        var property = await propertyService.UpdateAsync(request);

        return Ok(mapper.Map<PropertyResponse>(property));
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpDelete("{id}", Name = nameof(DeleteProperty))]
    [ProducesResponseType(typeof(OkObjectResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProperty([FromRoute] Guid id)
    {
        await propertyService.DeleteAsync(id);

        return Ok("Property deleted successfully");
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpGet("my", Name = nameof(GetMyProperties))]
    [ProducesResponseType(typeof(IEnumerable<PropertyResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyProperties()
    {
        var properties = await propertyService.GetMyAsync();

        return Ok(mapper.Map<List<PropertyResponse>>(properties));
    }

    [Authorize(nameof(AuthorizationLevel.Host))]
    [HttpDelete("host/delete/action", Name = nameof(DeleteHostProperties))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteHostProperties()
    {
        await propertyService.DeletePropertiesAsync();

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("search", Name = nameof(SearchProperties))]
    [ProducesResponseType(typeof(IEnumerable<SearchPropertyResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchProperties(
        [FromQuery] string location,
        [FromQuery] int guests,
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate)
    {
        var propertyResponses = await propertyService.SearchPropertiesAsync(location, guests, startDate, endDate);

        return Ok(propertyResponses);
    }
}
