using System;
using System.Collections.Generic;
using System.Text;
using Services.DTO.Building;

namespace Services.Interfaces
{
    public interface IBuildingService
    {
        Task<List<BuildingDTO>> GetAllAsync();
        Task<BuildingDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateBuildingDTO dto);
        Task<int> UpdateAsync(UpdateBuildingDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
