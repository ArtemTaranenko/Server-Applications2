using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO.EventType
{
    public class CreateEventTypeDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
