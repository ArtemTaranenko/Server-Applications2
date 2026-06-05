using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.EventType
{
	public class CreateEventTypeViewModel
	{
		[Display(Name="Nazwa")]
		[Required(ErrorMessage ="Pole Nazwa jest wymagane")]
		[MaxLength(100, ErrorMessage ="Pole Nazwa może mieć maksymalnie 100 znaków")]
		public string Name { get; set; } = null!;

		[Display(Name="Opis")]
		[MaxLength(200, ErrorMessage ="Opis może mieć maksymalnie 200 znaków")]
		public string? Description { get; set; }
	}
}
