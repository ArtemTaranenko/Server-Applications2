using System;
using System.Collections.Generic;
using System.Text;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Services.DTO.Equipment;
using Services.Interfaces;
using Model.DataModels;

namespace Services.Services
{
    public class EquipmentService: BaseService, IEquipmentService
    {
        public EquipmentService(MyDbContext dbContext): base(dbContext)
        {

        }
        public async Task<List<EquipmentDto>> GetAllAsync()
        {
            return await _dbContext.Equipments
                         .AsNoTracking()
                         .OrderBy(x => x.Name)
                         .Select(x => new EquipmentDto
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Description = x.Description,
                             IsMobile = x.IsMobile
                         })
                         .ToListAsync();
        }

        public async Task<EquipmentDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Equipments
                         .AsNoTracking()
                         .Where(x => x.Id == id)
                         .Select(x => new EquipmentDto
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Description = x.Description,
                             IsMobile = x.IsMobile
                         })
                         .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(EquipmentDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var equipment = new Equipment
            {
                Name = dto.Name,
                Description = dto.Description,
                IsMobile = dto.IsMobile
            };
            _dbContext.Equipments.Add(equipment);
            await _dbContext.SaveChangesAsync();
            return equipment.Id;
        }

        public async Task<bool> UpdateAsync(EquipmentDto dto)
        {
            var entity = await _dbContext.Equipments.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (entity == null)
                return false;
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsMobile = dto.IsMobile;

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
