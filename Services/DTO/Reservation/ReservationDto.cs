using Model.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.DTO.Reservation
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
        public int RoomId { get; set; }
        public int EventId { get; set; }
    }
}
