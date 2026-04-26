using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO.Building
{
    public class CreateBuildingDTO
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Description { get; set; }
    }
}
