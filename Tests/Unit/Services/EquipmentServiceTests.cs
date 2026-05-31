using System;
using System.Collections.Generic;
using System.Text;
using Services.Services;
using Services.DTO.Equipment;
using Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace Tests.Unit.Services
{
    public class EquipmentServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EquipmentService(dbContext, MapperFactory.Create());

            var result = await service.GetByIdAsync(99999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_New_Equipment()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EquipmentService(dbContext, MapperFactory.Create());

            var id = await service.CreateAsync(new CreateEquipmentDto
            {
                Name = "Test",
                Description = "Test",
                IsMobile = true
            });

            var created = await dbContext.Equipments.FirstOrDefaultAsync(x => x.Id == id);

            Assert.NotNull(created);
            Assert.Equal("Test", created.Name);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_False_When_Not_Found()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EquipmentService(dbContext, MapperFactory.Create());

            var updated = await service.UpdateAsync(new UpdateEquipmentDto
            {
                Id = 99999,
                Name = "Not found",
                Description = "Also not found",
                IsMobile = false
            });

            Assert.False(updated);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_When_Not_Assigned()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EquipmentService(dbContext, MapperFactory.Create());

            var laptop = await dbContext.Equipments.FirstOrDefaultAsync(x => x.Name == "Laptop");
            Assert.NotNull(laptop);

            var deleted = await service.DeleteAsync(laptop.Id);

            Assert.True(deleted);

            var isLaptopDeleted = await dbContext.Equipments.FirstOrDefaultAsync(x => x.Id == laptop.Id);
            Assert.Null(isLaptopDeleted);
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_When_Equipment_Is_Assigned_To_Room()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EquipmentService(dbContext, MapperFactory.Create());

            var equipmentId = await dbContext.Equipments
                .Where(x => x.Name == "Projector")
                .Select(x => x.Id)
                .FirstAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(equipmentId));
        }
    }
}
