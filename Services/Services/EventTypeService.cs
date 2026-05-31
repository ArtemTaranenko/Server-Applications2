using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Model.DataModels;
using Services.DTO.EventType;
using Services.Interfaces;

namespace Services.Services
{
    public class EventTypeService: BaseService, IEventTypeService
    {
        public EventTypeService(MyDbContext dbContext, IMapper mapper): base(dbContext, mapper)
        {

        }

        public async Task<List<EventTypeDto>> GetAllAsync()
        {
            return await _dbContext.EventTypes
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ProjectTo<EventTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EventTypeDto?> GetByIdAsync(int id)
        {
            return await _dbContext.EventTypes
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<EventTypeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateEventTypeDto dto)
        { 
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var eventType = _mapper.Map<EventType>(dto);
            _dbContext.EventTypes.Add(eventType);
            await _dbContext.SaveChangesAsync();
            return eventType.Id;
        }
        
        public async Task<bool> UpdateAsync (UpdateEventTypeDto dto)
        {
            var entity = await _dbContext.EventTypes.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
            _mapper.Map(dto, entity);
            
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
