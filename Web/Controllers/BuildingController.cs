using Microsoft.AspNetCore.Mvc;
using Services.Services;
using AutoMapper;

namespace Web.Controllers
{
	public class BuildingController : BaseController
	{
		private readonly BuildingService _buildingService;
		public BuildingController(IWebHostEnvironment env, BuildingService buildingService) : base(env)
		{
			_buildingService = buildingService;
		}

		public IActionResult Index()
		{
			var buildingsDtos = _buildingService.GetAllAsync();
			var buildings =  buildingsDtos.ProjectTo<

			return View();
		}
	}
}
