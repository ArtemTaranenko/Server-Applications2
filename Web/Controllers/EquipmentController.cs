using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.Equipment;
using Services.Interfaces;
using Web.ViewModels.Building;
using Web.ViewModels.Equipment;

namespace Web.Controllers
{
	public class EquipmentController : BaseController
	{
		private readonly IEquipmentService _equipmentService;
		private readonly IMapper _mapper;

		public EquipmentController(IEquipmentService equipmentService, IMapper mapper, IWebHostEnvironment env):base(env)
		{
			_equipmentService = equipmentService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var equipmentDtos = await _equipmentService.GetAllAsync();
			var equipmentVms = _mapper.Map<List<EquipmentViewModel>>(equipmentDtos);
			return View(equipmentVms);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateEquipmentViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);
			var equipmentDto = _mapper.Map<CreateEquipmentDto>(model);
			var result = await _equipmentService.CreateAsync(equipmentDto);

			SetSuccessMessage("Wyposażenie zostało utworzone");

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
		{
			var equipmentDto = await _equipmentService.GetByIdAsync(id);
			var equipmentVm = _mapper.Map<UpdateEquipmentViewModel>(equipmentDto);

			return View(equipmentVm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateEquipmentViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);
			var equipmentDto = _mapper.Map<UpdateEquipmentDto>(model);
			var result = await _equipmentService.UpdateAsync(equipmentDto);

			if (result == false){
				SetErrorMessage("Błąd przy próbie utworzenia wyposażenia");
				return RedirectToAction("Error", "Home");
			}

			SetSuccessMessage("Wyposażenie zostało utworzone");

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete (int id)
		{
			var equipmentDto = await _equipmentService.GetByIdAsync (id);
			var equipmentVm = _mapper.Map<DeleteEquipmentViewModel>(equipmentDto);

			return View(equipmentVm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete (DeleteBuildingViewModel model)
		{
			var result = await _equipmentService.DeleteAsync(model.Id);

			if (result == false)
			{
				SetErrorMessage("Błąd przy próbie usunięcia wyposażenia");
				return RedirectToAction("Error", "Home");
			}

			SetSuccessMessage("Wyposażenie zostało usunięte");

			return RedirectToAction("Index");
		}
	}
}

