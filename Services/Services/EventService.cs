using System;
using System.Collections.Generic;
using System.Text;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Model.DataModels;
using Services.DTO.Event;
using Services.DTO.Reservation;
using Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Services.DTO.Building;

namespace Services.Services
{
    public class EventService: BaseService, IEventService
    {
        EventService(MyDbContext dbContext, IMapper mapper): base(dbContext, mapper) { }

        public async Task<List<EventDto>> GetAllAsync()
        {
            return await _dbContext.Events
                         .AsNoTracking()
                         .OrderByDescending(x => x.CreatedAt)
                         .OrderBy(x => x.Title)
                         .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                         .ToListAsync();
        }

        public async Task<List<EventDto>> GetPublicEventsAsync()
        {
            return await _dbContext.Events
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .OrderBy(x => x.Title)
                        .Where(x => x.IsPublic == true)
                        .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();
        }
        
        public async Task<List<EventDto>> GetByEventTypeIdAsync(int eventTypeId)
        {
            return await _dbContext.Events
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .OrderBy(x => x.Title)
                        .Where(x => x.EventTypeId == eventTypeId)
                        .ProjectTo<EventDto>(_mapper.ConfigurationProvider)
                        .ToListAsync();
        }
        
        public async Task<EventDetailsDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Events
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .OrderBy(x => x.Title)
                        .Where(x => x.Id == id)
                        .ProjectTo<EventDetailsDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateEventDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var eventType = await _dbContext.EventTypes.FindAsync(dto.EventTypeId) ?? throw new InvalidOperationException(nameof(dto));
            if (dto.ParticipantsLimit <= 0)
                throw new InvalidOperationException(nameof(dto));
            var entity = _mapper.Map<Event>(dto);

            _dbContext.Events.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity.Id;
            
        }

        public async Task<bool> UpdateAsync(UpdateEventDto dto)
        {
            var entity = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
            if (dto.ParticipantsLimit <= 0)
                return false;
            entity = _mapper.Map<Event>(dto);

            var eventType = await _dbContext.EventTypes.FindAsync(dto.EventTypeId) ?? throw new InvalidOperationException(nameof(dto));

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContext.Events.FindAsync(id);
            if (entity == null)
                return false;
            if (entity.Reservations == null)
                throw new InvalidOperationException(nameof(entity));
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
