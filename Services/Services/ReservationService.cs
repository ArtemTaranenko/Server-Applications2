using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public ReservationService(MyDbContext dbContext, IMapper mapper): base(dbContext, mapper)
        {

        }
        public async Task<List<ReservationDto>> GetAllAsync()
        {
            return await _dbContext.Reservations
                                    .AsNoTracking()
                                    .ProjectTo<ReservationDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync();
        }
        public async Task<List<ReservationDto>> GetByRoomIdAsync(int roomId)
        {
            return await _dbContext.Reservations
                                    .AsNoTracking()
                                    .Where(x => x.RoomId == roomId)
                                    .ProjectTo<ReservationDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync();
        }
        public async Task<ReservationDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Reservations
                                   .AsNoTracking()
                                   .Where(x => x.Id == id)
                                   .ProjectTo<ReservationDto>(_mapper.ConfigurationProvider)
                                   .FirstOrDefaultAsync();
        }
        public async Task<int> CreateAsync(CreateReservationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var entity = _mapper.Map<Reservation>(dto);
            if (await HasTimeConflictAsync(entity.RoomId, entity.StartTime, entity.EndTime, entity.Id))
                throw new InvalidOperationException("Konflikt czasowy rezerwacji");
            if (await CanRoomAccomodateEventAsync(dto.RoomId, dto.EventId))
                throw new InvalidOperationException("Sala nie może umieścić określoną liczbę uczestników wydarzenia");
            if (dto.EndTime <= dto.StartTime)
                throw new InvalidOperationException(
                    "Czas zakończenia musi być późniejszy niż czas rozpoczęcia");

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
                return false;
            entity = _mapper.Map<Reservation>(dto);
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
