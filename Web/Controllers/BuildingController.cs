using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.Building;
using Services.Interfaces;
using Web.Mapping;
using Web.ViewModels.Building;
using Web.ViewModels.Room;

namespace Web.Controllers
{
	public class BuildingController : BaseController
	{
		private readonly IBuildingService _buildingService;
		private readonly IRoomService _roomService;
		private readonly IMapper _mapper;
		public BuildingController(IBuildingService buildingService,
			IRoomService roomService,
			IMapper mapper,
			IWebHostEnvironment env) : base(env)
		{
			_buildingService = buildingService;
			_roomService = roomService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var buildingsDtos = await _buildingService.GetAllAsync();
			var buildings = _mapper.Map<List<BuildingListItemViewModel>>(buildingsDtos);

			return View(buildings);
		}

		public async Task<IActionResult> Details(int id)
		{
			var buildingDto = await _buildingService.GetByIdAsync(id);
			var building = _mapper.Map<BuildingDetailsViewModel>(buildingDto);

			var roomsDto = await _roomService.GetByBuildingIdAsync(id);
			var rooms = _mapper.Map<List<RoomListItemViewModel>>(roomsDto);

			building.Rooms = rooms;

			return View(building);
		}
		
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateBuildingViewModel building)
		{
			if (!ModelState.IsValid)
				return View(building);
			var buildingDto = _mapper.Map<CreateBuildingDto>(building);
			var newBuilding = await _buildingService.CreateAsync(buildingDto);

			SetSuccessMessage("Budynek został utrworzony");
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
		{
			var buildingDto = await _buildingService.GetByIdAsync(id);
			var building = _mapper.Map<EditBuildingViewModel>(buildingDto);

			return View(building);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditBuildingViewModel building)
		{
			var buildingDto = _mapper.Map<UpdateBuildingDto>(building);
			var result = await _buildingService.UpdateAsync(buildingDto);

			if (!result)
				return RedirectToAction("Home", "Error");
			return RedirectToAction("Index");

		}

		public async Task<IActionResult> Delete(int id)
		{
			var buildingDto = await _buildingService.GetByIdAsync(id);
			var building = _mapper.Map<DeleteBuildingViewModel>(buildingDto);

			return View(building);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(DeleteBuildingViewModel building)
		{
			var result = await _buildingService.DeleteAsync(building.Id);

			if (!result)
				return RedirectToAction("Home", "Error");
			return RedirectToAction("Index");
		}
	}
}
