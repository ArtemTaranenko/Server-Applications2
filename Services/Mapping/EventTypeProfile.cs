using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Services.DTO.EventType;
using Model.DataModels;

namespace Services.Mapping
{
    public class EventTypeProfile: Profile
    {
        public EventTypeProfile()
        {
            CreateMap<EventType, EventTypeDto>();

            CreateMap<CreateEventTypeDto, EventType>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Events, o => o.Ignore());

            CreateMap<UpdateEventTypeDto, EventType>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Events, o => o.Ignore());
        }
    }
}
