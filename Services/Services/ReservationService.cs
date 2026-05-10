using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.DataModels;
using Services.DTO.Reservation;
using Services.Interfaces;

namespace Services.Services
{
    public class ReservationService: BaseService, IReservationService
    {
        public ReservationService(MyDbContext dbContext): base(dbContext)
        {

        }
        public async Task<List<ReservationDto>> GetAllAsync()
        {
            return await _dbContext.Reservations
                                    .AsNoTracking()
                                    .Select(x => new ReservationDto
                                    {
                                        Id = x.Id,
                                        StartTime = x.StartTime,
                                        EndTime = x.EndTime,
                                        Status = x.Status,
                                        CreatedAt = x.CreatedAt,
                                        Notes = x.Notes,
                                        RoomId = x.RoomId,
                                        EventId = x.EventId,
                                    })
                                    .ToListAsync();
        }
        public async Task<List<ReservationDto>> GetByRoomIdAsync(int roomId)
        {
            return await _dbContext.Reservations
                                    .AsNoTracking()
                                    .Where(x => x.RoomId == roomId)
                                    .Select(x => new ReservationDto
                                    {
                                        Id = x.Id,
                                        StartTime = x.StartTime,
                                        EndTime = x.EndTime,
                                        Status = x.Status,
                                        CreatedAt = x.CreatedAt,
                                        Notes = x.Notes,
                                        RoomId = x.RoomId,
                                        EventId = x.EventId,
                                    })
                                    .ToListAsync();
        }
        public async Task<ReservationDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Reservations
                                   .AsNoTracking()
                                   .Where(x => x.Id == id)
                                   .Select(x => new ReservationDto
                                   {
                                       Id = x.Id,
                                       StartTime = x.StartTime,
                                       EndTime = x.EndTime,
                                       Status = x.Status,
                                       CreatedAt = x.CreatedAt,
                                       Notes = x.Notes,
                                       RoomId = x.RoomId,
                                       EventId = x.EventId,
                                   })
                                   .FirstOrDefaultAsync();
        }
        public async Task<int> CreateAsync(CreateReservationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (await HasTimeConflictAsync(dto.RoomId, dto.StartTime, dto.EndTime, dto.Id))
                throw new InvalidOperationException("Konflikt czasowy rezerwacji");
            if (await CanRoomAccomodateEventAsync(dto.RoomId, dto.EventId))
                throw new InvalidOperationException("Sala nie może umieścić określoną liczbę uczestników wydarzenia");
            if (dto.EndTime <= dto.StartTime)
                throw new InvalidOperationException(
                    "Czas zakończenia musi być późniejszy niż czas rozpoczęcia");

            var entity = new Reservation
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status,
                CreatedAt = dto.CreatedAt,
                Notes = dto.Notes,
                RoomId = dto.RoomId,
                EventId = dto.EventId,
            };

            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;

        }
        public async Task<bool> UpdateAsync(UpdateReservationDto dto)
        {
            if (await HasTimeConflictAsync(dto.RoomId, dto.StartTime, dto.EndTime, dto.Id))
                throw new InvalidOperationException("Konflikt czasowy rezerwacji");
            if (await CanRoomAccomodateEventAsync(dto.RoomId, dto.EventId))
                throw new InvalidOperationException("Sala nie może umieścić określoną liczbę uczestników wydarzenia");
            if (dto.EndTime <= dto.StartTime)
                throw new InvalidOperationException(
                    "Czas zakończenia musi być późniejszy niż czas rozpoczęcia");
            var entity = await _dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;
            entity.Status = dto.Status;
            entity.CreatedAt = dto.CreatedAt;
            entity.Notes = dto.Notes;
            entity.RoomId = dto.RoomId;
            entity.EventId = dto.EventId;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContext.Reservations.FindAsync(id);
            if (entity == null)
                return false;

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task<bool> HasTimeConflictAsync(int roomId, DateTime startTime,
            DateTime endTime, int? reservationId = null)
        {
            return await _dbContext.Reservations
                                    .AnyAsync(x => x.RoomId == roomId &&

                                    (!reservationId.HasValue || x.Id != reservationId.Value) &&

                                    x.Status != ReservationStatus.Cancelled &&
                                    x.Status != ReservationStatus.Rejected &&

                                    startTime < x.EndTime &&
                                    endTime > x.StartTime

                                    );
        }

        private async Task<bool> CanRoomAccomodateEventAsync(int roomId, int eventId)
        {
            var room = await _dbContext.Rooms
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == roomId);
            if (room == null)
                throw new InvalidOperationException("Wybrana sala nie istnieje");
            if (room.IsActive == false)
                throw new InvalidOperationException("Wybrana sala nie jest aktywna");
            var ev = await _dbContext.Events
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == eventId);

            if (ev == null)
                throw new InvalidOperationException("Wybrane wydarzenie nie istnieje");

            return room.Capacity < ev.ParticipantsLimit;
        }
    }
}
