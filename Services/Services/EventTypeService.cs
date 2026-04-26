using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Model.DataModels;
using Services.DTO.EventType;
using Services.Interfaces;

namespace Services.Services
{
    public class EventTypeService: BaseService, IEventTypeService
    {
        public EventTypeService(MyDbContext dbContext): base(dbContext)
        {

        }

        public async Task<List<EventTypeDto>> GetAllAsync()
        {
            return await _dbContext.EventTypes
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new EventTypeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync();
        }

        public async Task<EventTypeDto?> GetByIdAsync(int id)
        {
            return await _dbContext.EventTypes
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new EventTypeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(EventTypeDto dto)
        { 
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var eventType = new EventType
            {
                Name = dto.Name,
                Description = dto.Description
            };
            _dbContext.EventTypes.Add(eventType);
            await _dbContext.SaveChangesAsync();
            return eventType.Id;
        }
        
        public async Task<bool> UpdateAsync (EventTypeDto dto)
        {
            var entity = await _dbContext.EventTypes.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync (int id)
        {
            var eventType = await _dbContext.EventTypes.FindAsync(id);
            if (eventType == null)
                return false;
            _dbContext.EventTypes.Remove(eventType);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
