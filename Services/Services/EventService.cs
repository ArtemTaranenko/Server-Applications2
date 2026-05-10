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
        
        public async Task<EventDetailsDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Events
                        .AsNoTracking()
                        .OrderByDescending(x => x.CreatedAt)
                        .OrderBy(x => x.Title)
                        .Where(x => x.Id == id)
                        .Select(x => new EventDetailsDto
                        {
                            Id = x.Id,
                            Title = x.Title,
                            Description = x.Description,
                            ParticipantsLimit = x.ParticipantsLimit,
                            IsPublic = x.IsPublic,
                            CreatedAt = x.CreatedAt,
                            EventTypeId = x.EventTypeId,
                            Reservations = x.Reservations
                                             .Select(x => new ReservationDto
                                             {
                                                 Id = x.Id,
                                                 StartTime = x.StartTime,
                                                 EndTime = x.EndTime,
                                                 Status = x.Status,
                                                 CreatedAt = x.CreatedAt,
                                                 Notes = x.Notes,
                                                 RoomId = x.RoomId,
                                                 EventId = x.EventId
                                             })
                                             .ToList()
                        })
                        .FirstOrDefaultAsync();
        }
        public async Task<int> CreateAsync(CreateEventDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var eventType = await _dbContext.EventTypes.FindAsync(dto.EventTypeId) ?? throw new InvalidOperationException(nameof(dto));
            if (dto.ParticipantsLimit <= 0)
                throw new InvalidOperationException(nameof(dto));
            var entity = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                ParticipantsLimit = dto.ParticipantsLimit,
                IsPublic = dto.IsPublic,
                EventTypeId = dto.EventTypeId,
                CreatedAt = DateTime.Now
            };
            return entity.Id;
            

        }
        public async Task<bool> UpdateAsync(UpdateEventDto dto)
        {
            var entity = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
            entity.Id = dto.Id;
            entity.Title = dto.Title;
            entity.Description = dto.Description;
            if (dto.ParticipantsLimit <= 0)
                return false;
            entity.ParticipantsLimit = dto.ParticipantsLimit;
            entity.IsPublic = dto.IsPublic;
            var eventType = await _dbContext.EventTypes.FindAsync(dto.EventTypeId) ?? throw new InvalidOperationException(nameof(dto));

            entity.EventTypeId = dto.EventTypeId;
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
