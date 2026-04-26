using System;
using System.Collections.Generic;
using System.Text;
using Services.DTO.EventType;


namespace Services.Interfaces
{
    public interface IEventTypeService
    {
        Task<List<EventTypeDto>> GetAllAsync();
        Task<EventTypeDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(EventTypeDto dto);
        Task<bool> UpdateAsync(EventTypeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
