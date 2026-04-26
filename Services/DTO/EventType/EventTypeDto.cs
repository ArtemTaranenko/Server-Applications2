using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO.EventType
{
    public class EventTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
