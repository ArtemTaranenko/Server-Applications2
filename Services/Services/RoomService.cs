using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Services.DTO.Room;
using Services.DTO.RoomEquipment;
using Services.Interfaces;
using Model.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class RoomService: BaseService, IRoomService
    {
        public RoomService(MyDbContext dbContext): base(dbContext)
        {

        }

        public async Task<List<RoomDto>> GetAllAsync()
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select( x => new RoomDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Capacity = x.Capacity,
                    Floor = x.Floor,
                    IsActive = x.IsActive,
                    BuildingId = x.BuildingId,
                    BuildingName = x.Building.Name
                })
                .ToListAsync();
        }

        public async Task<List<RoomDto>> GetByBuildingIdAsync (int id)
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .Where(x => x.BuildingId == id)
                .Select(x => new RoomDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Capacity = x.Capacity,
                    Floor = x.Floor,
                    IsActive = x.IsActive,
                    BuildingId = x.BuildingId,
                    BuildingName = x.Building.Name
                })
                .ToListAsync();
        }

        public async Task<List<RoomDto>> GetActiveRoomsAsync()
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .Select(x => new RoomDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Capacity = x.Capacity,
                    Floor = x.Floor,
                    IsActive = x.IsActive,
                    BuildingId = x.BuildingId,
                    BuildingName = x.Building.Name
                })
                .ToListAsync();
        }

        public async Task<RoomDetailsDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Rooms
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new RoomDetailsDto
                {
                    Name = x.Name,
                    Capacity = x.Capacity,
                    Floor = x.Floor,
                    IsActive = x.IsActive,
                    BuildingId = x.BuildingId,
                    BuildingName = x.Building.Name,
                    Equipment = x.RoomEquipments
                                .Select(e => new RoomEquipmentDto
                                {
                                    Id = e.Id,
                                    Quantity = e.Quantity,
                                    RoomId = e.RoomId,
                                    RoomName = e.Room.Name,
                                    EquipmentId = e.EquipmentId,
                                    EquipmentName = e.Equipment.Name
                                })
                                .ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateRoomDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var room = new Room
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                Floor = dto.Floor,
                IsActive= dto.IsActive,
                BuildingId = dto.BuildingId
            };
            _dbContext.Rooms.Add(room);
            await _dbContext.SaveChangesAsync();
            return room.Id;
        }

        public async Task<bool> UpdateAsync(UpdateRoomDto dto)
        {
            var entity = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
            entity.Name = dto.Name;
            entity.Capacity = dto.Capacity;
            entity.Floor = dto.Floor;
            entity.IsActive = dto.IsActive;
            entity.BuildingId = dto.BuildinId;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _dbContext.Rooms.FindAsync(id);
            if (room == null)
                return false;
            _dbContext.Rooms.Remove(room);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
