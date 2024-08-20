using AccommodationService.Application.Services;
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

    [HttpPost(Name = nameof(CreateProperty))]
    [ProducesResponseType(typeof(PropertyResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProperty([FromBody] PropertyRequest request)
    {
        var property = await propertyService.CreateAsync(mapper.Map<PropertyEntity>(request));

        return CreatedAtAction(nameof(CreateProperty), mapper.Map<PropertyResponse>(property));
    }
}
