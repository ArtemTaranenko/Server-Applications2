using System;
using System.Collections.Generic;
using System.Text;
using Services.DTO.Reservation;

namespace Services.Interfaces
{
    public interface IReservationService
    {
    Task<List<ReservationDto>> GetAllAsync();
    Task<List<ReservationDto>> GetByRoomIdAsync(int roomId);
    Task<ReservationDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateReservationDto dto);
    Task<bool> UpdateAsync(UpdateReservationDto dto);
    Task<bool> DeleteAsync(int id);
    
    }

}
