using AutoMapper;
using Model.DataModels;
using Services.DTO.Room;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Mapping
{
    public class RoomProfile: Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>();

            CreateMap<Room, RoomDetailsDto>()
                .ForMember(d => d.BuildingName, o => o.MapFrom(src => src.Building.Name));

            CreateMap<CreateRoomDto, Room>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Reservations, o => o.Ignore())
                .ForMember(d => d.RoomEquipments, o => o.Ignore())
                .ForMember(d => d.Building, o => o.Ignore());

            CreateMap<UpdateRoomDto, Room>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Reservations, o => o.Ignore())
                .ForMember(d => d.RoomEquipments, o => o.Ignore())
                .ForMember(d => d.Building, o => o.Ignore());
        }
    }
}
