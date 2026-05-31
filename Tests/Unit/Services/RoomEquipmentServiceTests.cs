using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Services.Services;
using Services.DTO.RoomEquipment;
using Tests.TestInfrastructure;
using Microsoft.VisualBasic;

namespace Tests.Unit.Services
{
    public class RoomEquipmentServiceTests
    {
        [Fact]
        public async Task GetByRoomIdAsync_Should_Return_Assigned_Equipment()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomEquipmentService(dbContext, MapperFactory.Create());

            var roomId = await dbContext.Rooms
                .Where(x => x.Name == "A-101")
                .Select(x => x.Id)
                .FirstAsync();

            var equipmentId = await dbContext.Equipments
                .Where(x => x.Name == "Projector")
                .Select(x => x.Id)
                .FirstAsync();

            var result = await service.GetByRoomIdAsync(roomId);

            Assert.Contains(result, x => x.EquipmentId == equipmentId);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Assignment_Already_Exists()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomEquipmentService(dbContext, MapperFactory.Create());

            var roomId = await dbContext.Rooms
                .Where(x => x.Name == "A-101")
                .Select(x => x.Id)
                .FirstAsync();

            var equipmentId = await dbContext.Equipments
                .Where(x => x.Name == "Projector")
                .Select(x => x.Id)
                .FirstAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new CreateRoomEquipmentDto
            {
                RoomId = roomId,
                EquipmentId = equipmentId,
                Quantity = 1
            }));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public async Task CreateAsync_Should_Throw_When_Quantity_Invalid(int quantity)
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomEquipmentService(dbContext, MapperFactory.Create());

            var roomId = await dbContext.Rooms
                .Where(x => x.Name == "A-101")
                .Select(x => x.Id)
                .FirstAsync();

            var equipmentId = await dbContext.Equipments
                .Where(x => x.Name == "Speakers")
                .Select(x => x.Id)
                .FirstAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new CreateRoomEquipmentDto
            {
                RoomId = roomId,
                EquipmentId = equipmentId,
                Quantity = quantity
            }));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Quantity()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomEquipmentService(dbContext, MapperFactory.Create());

            var roomId = await dbContext.Rooms
                .Where(x => x.Name == "A-101")
                .Select(x => x.Id)
                .FirstAsync();

            var roomEquipmentId = await dbContext.RoomEquipments
                .Where(x => x.RoomId == roomId)
                .Select(x => x.Id)
                .FirstAsync();

            var updated = await service.UpdateAsync(new UpdateRoomEquipmentDto
            {
                Id = roomEquipmentId,
                Quantity = 3
            });

            Assert.True(updated);
            var roomEquipment = await dbContext.RoomEquipments.FirstAsync(x => x.Id == roomEquipmentId); ;
            Assert.Equal(3, roomEquipment.Quantity);
        }

        [Fact]
        public async Task DeleteAsync_Shoudl_Return_False_When_Not_Found()
        {
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new RoomEquipmentService(dbContext, MapperFactory.Create());

            var deleted = await service.DeleteAsync(99999);

            Assert.False(deleted);
		}
    }
}
