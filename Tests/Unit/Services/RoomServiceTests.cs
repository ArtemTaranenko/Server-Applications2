using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Services.DTO.Room;
using Services.Services;
using Tests.TestInfrastructure;

namespace Tests.Unit.Services
{
    public class RoomServiceTests
    {
        [Fact]
        public async Task CreateAsync_Should_Throw_When_Building_Not_Exists()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomService(dbContext, MapperFactory.Create());

            var room = new CreateRoomDto
            {
                Name = "Test Room",
                Capacity = 10,
                Floor = 1,
                IsActive = true,
                BuildingId = 99999
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(room));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CreateAsync_Should_Throw_When_Capacity_Is_Invalid(int capacity)
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomService(dbContext, MapperFactory.Create());

            var buildingId = await dbContext.Buildings
                .Where(x => x.Name == "Test Building C")
                .Select(x => x.Id)
                .FirstAsync();

            var room = new CreateRoomDto
            {
                Name = "Test Room",
                Capacity = capacity,
                Floor = 1,
                IsActive = true,
                BuildingId = buildingId
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(room));
        }

        [Fact]
        public async Task GetActiveRoomsAsync_Should_Return_Only_Active_Rooms()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomService(dbContext, MapperFactory.Create());

            var result = await service.GetActiveRoomsAsync();

            Assert.All(result, room => Assert.True(room.IsActive));
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_False_When_Not_Found()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomService(dbContext, MapperFactory.Create());

            var buildingId = await dbContext.Buildings
                .Where(x => x.Name == "Test Building B")
                .Select(x => x.Id)
                .FirstAsync();

            var updated = await service.UpdateAsync(new UpdateRoomDto
            {
                Id = 99999,
                Name = "Not found",
                Capacity = 1,
                Floor = 1,
                IsActive = true,
                BuildingId = buildingId
            });

            Assert.False(updated);
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_When_Room_Has_Reservations()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new RoomService(dbContext, MapperFactory.Create());

            var roomId = await dbContext.Rooms
                .Where(x => x.Name == "A-101")
                .Select(x => x.Id)
                .FirstAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(()  => service.DeleteAsync(roomId));
        }
    }
}
