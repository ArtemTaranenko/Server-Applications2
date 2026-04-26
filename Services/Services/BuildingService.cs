using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DAL.EF;
using Services.DTO.Building;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Model.DataModels;

namespace Services.Services
{
    public class BuildingService : BaseService, IBuildingService
    {
        public BuildingService(MyDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<BuildingDto>> GetAllAsync()
        {
            return await _dbContext.Buildings
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Select(x => new BuildingDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Description = x.Description
                })
                .ToListAsync();
        }

        public async Task<BuildingDto?> GetByIdAsync(int id)
        {
            return await _dbContext.Buildings
                            .AsNoTracking()
                            .Where(x => x.Id == id)
                            .Select(x => new BuildingDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Address = x.Address,
                                Description = x.Description
                            })
                            .FirstOrDefaultAsync();
        }
        
        public async Task<int> CreateAsync(CreateBuildingDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            var building = new Building
            {
                Name = dto.Name,
                Address = dto.Address,
                Description = dto.Description
            };

            _dbContext.Buildings.Add(building);
            await _dbContext.SaveChangesAsync();
            return building.Id;
        }

        public async Task<bool> UpdateAsync(UpdateBuildingDto dto)
        {
            var entity = await _dbContext.Buildings.FirstOrDefaultAsync(x =>x.Id == dto.Id);
            if (entity == null)
                return false;
            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.Description = dto.Description;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var building = await _dbContext.Buildings.FindAsync(id);

            if (building == null)
                return false;
            _dbContext.Buildings.Remove(building);
            await _dbContext.SaveChangesAsync();
            return true;

        }
    }
}

