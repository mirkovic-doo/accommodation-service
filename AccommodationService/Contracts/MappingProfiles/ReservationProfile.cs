using AccommodationService.Controllers.Reservation.Requests;
using AccommodationService.Controllers.Reservation.Responses;
using AccommodationService.Domain;
using AutoMapper;

namespace AccommodationService.Contracts.MappingProfiles;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<ReservationRequest, Reservation>();
        CreateMap<Reservation, ReservationResponse>();
    }
}
