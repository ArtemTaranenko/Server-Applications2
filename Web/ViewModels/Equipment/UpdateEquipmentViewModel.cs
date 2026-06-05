namespace Web.ViewModels.Equipment
{
	public class UpdateEquipmentViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public bool IsMobile { get; set; }
	}
}
