using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.DTO.EventType;
using Services.Interfaces;
using Web.ViewModels.EventType;

namespace Web.Controllers
{
	public class EventTypeController : BaseController
	{
		private readonly IEventTypeService _eventTypeService;
		private readonly IMapper _mapper;

		public EventTypeController(IEventTypeService eventTypeService, IMapper mapper, IWebHostEnvironment env): base(env)
		{
			_eventTypeService = eventTypeService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var eventTypeDtos = await _eventTypeService.GetAllAsync();
			var eventTypeVms = _mapper.Map<List<EventTypeViewModel>>(eventTypeDtos);

			return View(eventTypeVms);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateEventTypeViewModel model)
		{
			if (!ModelState.IsValid)
				RedirectToAction("Error", "Home");

			var eventType = _mapper.Map<CreateEventTypeDto>(model);

			var result = await _eventTypeService.CreateAsync(eventType);

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
		{
			var eventTypeDto = await _eventTypeService.GetByIdAsync(id);
			var eventTypeVm = _mapper.Map<UpdateEventTypeViewModel>(eventTypeDto);

			return View(eventTypeVm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(UpdateEventTypeViewModel model)
		{
			if (!ModelState.IsValid)
				return RedirectToAction("Error", "Home");
			var eventTypeDto = _mapper.Map<UpdateEventTypeDto>(model);

			var result = await _eventTypeService.UpdateAsync(eventTypeDto);
			if (result == false)
				return RedirectToAction("Error", "Home");

			SetSuccessMessage("Typ wydarzenia został uaktualniony");
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var eventTypeDto = await _eventTypeService.GetByIdAsync(id);
			var eventTypeVm = _mapper.Map<DeleteEventTypeViewModel>(eventTypeDto);

			return View(eventTypeVm);			
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(DeleteEventTypeViewModel model)
		{

			var result = await _eventTypeService.DeleteAsync(model.Id);

			if (result == false)
				return RedirectToAction("Error", "Home");

			SetSuccessMessage("Usunięto typ wydarzenia");

			return RedirectToAction("Index");
		}

	}
}

