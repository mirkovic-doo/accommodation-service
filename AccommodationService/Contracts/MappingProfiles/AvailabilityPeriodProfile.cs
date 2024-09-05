using AccommodationService.Controllers.AvailabilityPeriod.Requests;
using AccommodationService.Controllers.AvailabilityPeriod.Responses;
using AccommodationService.Domain;
using AutoMapper;

namespace AccommodationService.Contracts.MappingProfiles;

public class AvailabilityPeriodProfile : Profile
{
    public AvailabilityPeriodProfile()
    {
        CreateMap<AvailabilityPeriodRequest, AvailabilityPeriod>();
        CreateMap<AvailabilityPeriod, AvailabilityPeriodResponse>();
    }
}
