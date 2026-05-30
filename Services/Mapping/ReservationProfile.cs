using AutoMapper;
using Model.DataModels;
using Services.DTO.Reservation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Mapping
{
    public class ReservationProfile: Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationDto>();

            CreateMap<CreateReservationDto, Reservation>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Event, o => o.Ignore())
                .ForMember(d => d.Room, o => o.Ignore());
            
            CreateMap<UpdateReservationDto, Reservation>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Event, o => o.Ignore())
                .ForMember(d => d.Room, o => o.Ignore());
        }
    }
}
