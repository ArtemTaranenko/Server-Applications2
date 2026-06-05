using AutoMapper;
using Services.DTO.Equipment;
using Web.ViewModels.Equipment;

namespace Web.Mapping
{
	public class EquipmentViewModelProfile:Profile
	{
		public EquipmentViewModelProfile() 
		{ 
			CreateMap<EquipmentDto, EquipmentViewModel>();
			CreateMap<EquipmentDto, UpdateEquipmentViewModel>();
			CreateMap<EquipmentDto, DeleteEquipmentViewModel>();

			CreateMap<UpdateEquipmentViewModel, UpdateEquipmentDto>();
			CreateMap<CreateEquipmentViewModel, CreateEquipmentDto>();
		}
	}
}
