using System.Diagnostics;

namespace Model.DataModels
{
    public class Event
    {
        public int Id {  get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ParticipantsLimit { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt {  get; set; }
        public int EventTypeId { get; set; }
        public virtual EventType EventType { get; set; } = null!;
        public virtual List<Reservation>? Reservations { get; set; }
    }
}
