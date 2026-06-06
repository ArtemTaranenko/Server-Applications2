using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Equipment
{
	public class CreateEquipmentViewModel
	{
		[Display (Name="Nazwa")]
		[Required(ErrorMessage ="Nazwa jest wymagana!")]
		[MaxLength(100, ErrorMessage ="Nazwa może posiadać maksymalnie 100 znaków")]
		public string Name { get; set; } = null!;
		[Display(Name = "Opis")]
		[MaxLength(200, ErrorMessage = "Opis może posiadać maksymalnie 200 znaków")]
		public string? Description { get; set; }
		[Required(ErrorMessage = "Przenośność jest wymagana!")]
		public bool? IsMobile { get; set; }
	}
}
