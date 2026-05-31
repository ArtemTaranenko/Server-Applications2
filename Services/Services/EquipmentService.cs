using System;
using System.Collections.Generic;
using System.Text;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Services.DTO.Equipment;
using Services.Interfaces;
using Model.DataModels;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Services.Services
{
    public class EquipmentService: BaseService, IEquipmentService
    {
        public EquipmentService(MyDbContext dbContext, IMapper mapper): base(dbContext, mapper)
        {

        }
        public async Task<List<EquipmentDto>> GetAllAsync()
        {
            return await _dbContext.Equipments
                         .AsNoTracking()
                         .OrderBy(x => x.Name)
                         .ProjectTo<EquipmentDto>(_mapper.ConfigurationProvider)
                         .ToListAsync();
        }

        public async Task<EquipmentDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Equipments
                         .AsNoTracking()
                         .Where(x => x.Id == id)
                         .ProjectTo<EquipmentDto>(_mapper.ConfigurationProvider)
                         .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateEquipmentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var equipment = _mapper.Map<Equipment>(dto);
            _dbContext.Equipments.Add(equipment);
            await _dbContext.SaveChangesAsync();
            return equipment.Id;
        }

        public async Task<bool> UpdateAsync(UpdateEquipmentDto dto)
        {
            var entity = await _dbContext.Equipments.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
            _mapper.Map(dto, entity);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var equipment = await _dbContext.Equipments.FindAsync(id);
            if (equipment == null)
                return false;
            _dbContext.Remove(equipment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
