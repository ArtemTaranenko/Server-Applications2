using Services.DTO.RoomEquipment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IRoomEquipmentService
    {
        Task<List<RoomEquipmentDto>> GetAllAsync();
        Task<List<RoomEquipmentDto>> GetByRoomIdAsync(int roomId);
        Task<RoomEquipmentDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateRoomEquipmentDto dto);
        Task<bool> UpdateAsync(UpdateRoomEquipmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
