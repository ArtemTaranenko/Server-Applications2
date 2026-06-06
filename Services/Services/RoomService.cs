using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Services.DTO.Room;
using Services.DTO.RoomEquipment;
using Services.Interfaces;
using Model.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Services.Services
{
    public class RoomService: BaseService, IRoomService
    {
        public RoomService(MyDbContext dbContext, IMapper mapper): base(dbContext, mapper)
        {

        }

        public async Task<List<RoomDto>> GetAllAsync()
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ProjectTo<RoomDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<RoomDto>> GetByBuildingIdAsync (int id)
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .Where(x => x.BuildingId == id)
                .ProjectTo<RoomDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<RoomDto>> GetActiveRoomsAsync()
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .ProjectTo<RoomDto>( _mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<RoomDetailsDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .Where(x => x.Id == id)
                .ProjectTo<RoomDetailsDto?>( _mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateRoomDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (!await _dbContext.Buildings.AnyAsync(x => x.Id == dto.BuildingId))
                throw new InvalidOperationException("Nie można dodać pokoju w nieistniejącym budynku");

            if (dto.Capacity <= 0)
                throw new InvalidOperationException("Nieprawidłowa pojemność pokoju");

            var room = _mapper.Map<Room>(dto);

            _dbContext.Rooms.Add(room);
            await _dbContext.SaveChangesAsync();
            return room.Id;
        }

        public async Task<bool> UpdateAsync(UpdateRoomDto dto)
        {
            var entity = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
            entity = _mapper.Map<Room>(dto);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _dbContext.Rooms.FindAsync(id);
            if (room == null)
                return false;
            if (await _dbContext.Reservations.AnyAsync(x => x.RoomId == id))
                throw new InvalidOperationException("Nie można usunąć pokoju, który posiada rezerwacji");
            _dbContext.Rooms.Remove(room);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
