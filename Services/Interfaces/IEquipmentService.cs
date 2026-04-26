using System;
using System.Collections.Generic;
using System.Text;
using Services.DTO.Equipment;

namespace Services.Interfaces
{
    public interface IEquipmentService
    {
        Task<List<EquipmentDto>> GetAllAsync();
        Task<EquipmentDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(EquipmentDto dto);
        Task<bool> UpdateAsync(EquipmentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
