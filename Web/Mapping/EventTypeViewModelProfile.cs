using AutoMapper;
using Services.DTO.EventType;
using Web.ViewModels.EventType;

namespace Web.Mapping
{
	public class EventTypeViewModelProfile:Profile
	{
		public EventTypeViewModelProfile()
		{
			CreateMap<EventTypeDto, EventTypeViewModel>();
			CreateMap<EventTypeDto, UpdateEventTypeViewModel>();
			CreateMap<EventTypeDto, DeleteEventTypeViewModel>();

			CreateMap<EventTypeViewModel, EventTypeDto>();
			CreateMap<CreateEventTypeViewModel, CreateEventTypeDto>();
			CreateMap<UpdateEventTypeViewModel, UpdateEventTypeDto>();
		}
	}
}
