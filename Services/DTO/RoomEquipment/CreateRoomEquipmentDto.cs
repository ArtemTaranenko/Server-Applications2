using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO.RoomEquipment
{
    public class CreateRoomEquipmentDto
    {
        public int Quantity { get; set; }
        public int RoomId { get; set; }
        public int EquipmentId { get; set; }
    }
}
