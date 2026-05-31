using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public RoomEquipmentService(MyDbContext dbContext, IMapper mapper): base(dbContext, mapper)
        {

        }

        public async Task<List<RoomEquipmentDto>> GetAllAsync()
        {
            return await _dbContext.RoomEquipments
                .AsNoTracking()
                .OrderBy(x => x.Room.Building.Name)
                .ThenBy(x => x.Room.Name)
                .ThenBy(x => x.Equipment.Name)
                .ProjectTo<RoomEquipmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<RoomEquipmentDto>> GetByRoomIdAsync(int roomId)
        {
            return await _dbContext.RoomEquipments
                .AsNoTracking()
                .Where(x => x.RoomId == roomId)
                .OrderBy(x => x.Room.Building.Name)
                .ThenBy(x => x.Room.Name)
                .ThenBy(x => x.Equipment.Name)
                .ProjectTo<RoomEquipmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<RoomEquipmentDto?> GetByIdAsync(int id)
        {
            return await _dbContext.RoomEquipments
                .AsNoTracking()
                .Where(x => x.Id == id)
                .OrderBy(x => x.Room.Building.Name)
                .ThenBy(x => x.Room.Name)
                .ThenBy(x => x.Equipment.Name)
                .ProjectTo<RoomEquipmentDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
        public async Task<int> CreateAsync(CreateRoomEquipmentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var roomEquipment = _mapper.Map<CreateRoomEquipmentDto, RoomEquipment>(dto);

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

            if (dto.Quantity <= 0)
                throw new InvalidOperationException("Ilość wyposażenia musi być większa od zera");

            _mapper.Map(dto, entity);
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
