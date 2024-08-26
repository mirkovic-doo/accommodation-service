using AccommodationService.Controllers.Reservation.Requests;
using AccommodationService.Controllers.Reservation.Responses;
using AccommodationService.Domain;
using AccommodationService.Domain.Enums;
using AutoMapper;

namespace AccommodationService.Contracts.MappingProfiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<ReservationRequest, Reservation>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(o => ReservationStatus.Pending));

        CreateMap<Reservation, ReservationResponse>();
    }
}
