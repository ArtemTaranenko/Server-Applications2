using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DataModels
{
    public class RoomEquipment
    {
        public int Id {  get; set; }
        public int Quantity { get; set; }
        public int RoomId { get; set; }
        public virtual Room? Room { get; set; }
        public int EquipmentId { get; set; }
        public virtual Equipment? Equipment { get; set; }
    }
}
