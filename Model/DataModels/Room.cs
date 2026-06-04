using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DataModels
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public int Floor { get; set; }
        public bool IsActive { get; set; }
        public int BuildingId { get; set; }
        public virtual Building Building { get; set; } = null!;
        public virtual List<Reservation>? Reservations { get; set; }
        public virtual List<RoomEquipment> RoomEquipments { get; set; } = null!;
    }
}
