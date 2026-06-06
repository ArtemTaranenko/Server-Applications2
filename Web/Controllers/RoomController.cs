using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model.DataModels;
using Services.DTO.Room;
using Services.Interfaces;
using Web.ViewModels.Room;

namespace Web.Controllers
{
	public class RoomController : BaseController
	{
		private readonly IRoomService _roomService;
		private readonly IBuildingService _buildingService;
		private readonly IMapper _mapper;

		public RoomController(IRoomService roomService, IBuildingService buildingService, IMapper mapper, IWebHostEnvironment env): base(env)
		{
			_roomService = roomService;
			_buildingService = buildingService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var roomDtos = await _roomService.GetAllAsync();
			var roomVms = _mapper.Map<List<RoomListItemViewModel>>(roomDtos);

			return View(roomVms);
		}

		public async Task<IActionResult> Create()
		{
			var model = new CreateRoomViewModel
			{
				Buildings = await CreateBuildingSelectListAsync(null)
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateRoomViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Buildings = await CreateBuildingSelectListAsync(model.BuildingId);
				return View(model);
			}

			var dto = _mapper.Map<CreateRoomDto>(model);
			await _roomService.CreateAsync(dto);
			SetSuccessMessage("Sala została utworzona");
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Details(int id)
		{
			var roomDto = await _roomService.GetByIdAsync(id);
			var roomVm = _mapper.Map<RoomDetailsViewModel>(roomDto);

			return View(roomVm);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var roomDto = await _roomService.GetByIdAsync(id);
			var roomVm = _mapper.Map<EditRoomViewModel>(roomDto);
			roomVm.Buildings = await CreateBuildingSelectListAsync(null);

			return View(roomVm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditRoomViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Buildings = await CreateBuildingSelectListAsync(model.BuildingId);
				return View(model);
			}
				

			var roomDto = _mapper.Map<UpdateRoomDto>(model);
			await _roomService.UpdateAsync(roomDto);

			SetSuccessMessage("Sala została zaktualizowana");
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var roomDto = await _roomService.GetByIdAsync(id);
			var roomVm = _mapper.Map<DeleteRoomViewModel>(roomDto);

			return View(roomVm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(DeleteRoomViewModel model)
		{
			try
			{
				await _roomService.DeleteAsync(model.Id);
			}
			catch (InvalidOperationException e)
			{
				SetErrorMessage(e.Message);
				return RedirectToAction("Delete");
			}

			SetSuccessMessage("Sala została usunięta");

			return RedirectToAction("Index");
		}

		private async Task<List<SelectListItem>> CreateBuildingSelectListAsync(int? selectedId)
		{
			var buildings = await _buildingService.GetAllAsync();
			return buildings
				.Select(b => new SelectListItem
				{
					Value = b.Id.ToString(),
					Text = b.Name,
					Selected = selectedId.HasValue && b.Id == selectedId.Value
				})
				.ToList();
		}
	}
}
