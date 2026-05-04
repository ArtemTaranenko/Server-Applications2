using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO.Event
{
    public class UpdateEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ParticipantsLimit { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public int EventTypeId { get; set; }
    }
}
