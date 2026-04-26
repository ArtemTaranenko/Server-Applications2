using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO.Equipment
{
    public class CreateEquipmentDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsMobile { get; set; }
    }
}
