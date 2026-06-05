using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DataModels
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Description { get; set; }
        public virtual List<Room>? Rooms { get; set; }
    }
}
