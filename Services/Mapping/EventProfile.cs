using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Model.DataModels;
using Services.DTO.Event;

namespace Services.Mapping
{
    public class EventProfile: Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventDto>();

            CreateMap<Event, EventDetailsDto>()
                .ForMember(d => d.Reservations, o => o.MapFrom(d => d.Reservations));

            CreateMap<CreateEventDto, Event>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Reservations, o => o.Ignore())
                .ForMember(d => d.EventType, o => o.Ignore());

            CreateMap<UpdateEventDto, Event>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Reservations, o => o.Ignore())
                .ForMember(d => d.EventType, o => o.Ignore());
        }
    }
}
