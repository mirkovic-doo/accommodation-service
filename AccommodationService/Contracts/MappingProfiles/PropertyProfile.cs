﻿using AccommodationService.Controllers.Property.Requests;
using AccommodationService.Controllers.Property.Responses;
using AccommodationService.Domain;
using AutoMapper;

namespace AccommodationService.Contracts.MappingProfiles;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        CreateMap<PropertyRequest, Property>();
        CreateMap<Property, PropertyResponse>();
        CreateMap<Property, SearchPropertyResponse>()
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
            .ForMember(dest => dest.UnitPrice, opt => opt.Ignore());
    }
}
