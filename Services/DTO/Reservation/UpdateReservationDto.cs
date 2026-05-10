using System;
using System.Collections.Generic;
using System.Text;
using Model.DataModels;

namespace Services.DTO.Reservation
{
    public class UpdateReservationDto
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
