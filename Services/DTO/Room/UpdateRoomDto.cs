using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO.Room
{
    public class UpdateRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public int Floor { get; set; }
        public bool IsActive { get; set; }
        public int BuildinId {  get; set; }
    }
}
