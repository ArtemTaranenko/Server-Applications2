using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Model.DataModels;
using Services.DTO.RoomEquipment;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    public class RoomEquipmentService: BaseService, IRoomEquipmentService
    {
        RoomEquipmentService(MyDbContext dbContext): base(dbContext)
        {

        }

        public async Task<List<RoomEquipmentDto>> GetAllAsync()
        {
            return await _dbContext.RoomEquipments
                         .AsNoTracking()
                         .Select(x => new RoomEquipmentDto
                         {
                             Id = x.Id,
                             Quantity = x.Quantity,
                             RoomId = x.RoomId,
                             RoomName = x.Room.Name,
                             EquipmentId = x.EquipmentId,
                             EquipmentName = x.Equipment.Name
                         })
                         .ToListAsync();
        }

        public async Task<List<RoomEquipmentDto>> GetByRoomIdAsync(int roomId)
        {
            return await _dbContext.RoomEquipments
                .AsNoTracking()
                .Where(x => x.RoomId == roomId)
                .Select(x => new RoomEquipmentDto
                {
                    Id = x.Id,
                    Quantity = x.Quantity,
                    RoomId = x.RoomId,
                    RoomName = x.Room.Name,
                    EquipmentId = x.EquipmentId,
                    EquipmentName = x.Equipment.Name
                })
                .ToListAsync();
        }

        public async Task<RoomEquipmentDto?> GetByIdAsync(int id)
        {
            return await _dbContext.RoomEquipments
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new RoomEquipmentDto
                {
                    Id = x.Id,
                    Quantity = x.Quantity,
                    RoomId = x.RoomId,
                    RoomName = x.Room.Name,
                    EquipmentId = x.EquipmentId,
                    EquipmentName = x.Equipment.Name
                })
                .FirstOrDefaultAsync();
        }
        public async Task<int> CreateAsync(CreateRoomEquipmentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var roomEquipment = new RoomEquipment
            {
                Quantity = dto.Quantity,
                RoomId = dto.RoomId,
                EquipmentId = dto.EquipmentId
            };

            _dbContext.RoomEquipments.Add(roomEquipment);
            await _dbContext.SaveChangesAsync();
            return roomEquipment.Id;
        }

        public async Task<bool> UpdateAsync(UpdateRoomEquipmentDto dto)
        { 
            var entity = await _dbContext.RoomEquipments
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
           
            entity.Id = dto.Id;
            entity.Quantity = dto.Quantity;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbContext.RoomEquipments.FindAsync(id);
            if (entity == null)
                return false;

            _dbContext.RoomEquipments.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
