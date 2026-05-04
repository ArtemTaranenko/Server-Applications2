using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Model.DataModels;
using Services.DTO.Event;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class EventService: BaseService, IEventService
    {
        EventService(MyDbContext dbContext): base(dbContext) { }

        public async Task<List<EventDto>> GetAllAsync()
        {
            return await _dbContext.Events
                         .AsNoTracking()
                         .OrderByDescending(x => x.CreatedAt)
                         .OrderBy(x => x.Title)
                         .Select(x => new EventDto
                         {
                             Id = x.Id,
                             Title = x.Title,
                             Description = x.Description,
                             ParticipantsLimit = x.ParticipantsLimit,
                             IsPublic = x.IsPublic,
                             CreatedAt = x.CreatedAt,
                             EventTypeId = x.EventTypeId
                         })
                         .ToListAsync();
        }

        public async Task<List<EventDto>> GetPublicEventsAsync()
        {
            return await _dbContext.Events
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .OrderBy(x => x.Title)
                        .Where(x => x.IsPublic == true)
                        .Select(x => new EventDto
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            ParticipantsLimit = x.ParticipantsLimit,
                            IsPublic = x.IsPublic,
                            CreatedAt = x.CreatedAt,
                            EventTypeId = x.EventTypeId
                        })
                        .ToListAsync();
        }
        
        public async Task<List<EventDto>> GetByEventTypeIdAsync(int eventTypeId)
        {
            return await _dbContext.Events
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .OrderBy(x => x.Title)
                        .Where(x => x.EventTypeId == eventTypeId)
                        .Select(x => new EventDto
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            ParticipantsLimit = x.ParticipantsLimit,
                            IsPublic = x.IsPublic,
                            CreatedAt = x.CreatedAt,
                            EventTypeId = x.EventTypeId
                        })
                        .ToListAsync();
        }
        
        public async Task<EventDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Events
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .OrderBy(x => x.Title)
                        .Where(x => x.Id == id)
                        .Select(x => new EventDto
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            ParticipantsLimit = x.ParticipantsLimit,
                            IsPublic = x.IsPublic,
                            CreatedAt = x.CreatedAt,
                            EventTypeId = x.EventTypeId
                        })
                        .FirstOrDefaultAsync();
        }
        public async Task<int> CreateAsync(CreateEventDto dto)
        {

        }
        public async Task<bool> UpdateAsync(UpdateEventDto dto)
        {

        }
        public async Task<bool> DeleteAsync(int id)
        {

        }
    }
}
