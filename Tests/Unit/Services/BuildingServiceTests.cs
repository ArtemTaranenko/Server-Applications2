using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Services.DTO.Building;
using Services.Services;
using Tests.TestInfrastructure;

namespace Tests.Services
{
    public class BuildingServiceTests
    {
        [Fact]
        public async Task GetAllAsync_Should_Return_Seeded_Buildings()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new BuildingService(dbContext, MapperFactory.Create());

            var result = await service.GetAllAsync();

            Assert.True(result.Count >= 3);
            Assert.Contains(result, x => x.Name == "Test Building A");
            Assert.Contains(result, x => x.Name == "Test Building B");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            await using var dbContext = TestDbContextFactory.Create();
            var service = new BuildingService(dbContext, MapperFactory.Create());

            var result = await service.GetByIdAsync(99999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Building_And_Return_Id()
        {
            await using var dbContext = TestDbContextFactory.Create();
            var service = new BuildingService(dbContext, MapperFactory.Create());

            var id = await service.CreateAsync(new CreateBuildingDto
            {
                Name = "Nowy budynek",
                Address = "ul. Testowa 1",
                Description = "Opis testowy"
            });

            var created = await dbContext.Buildings.FirstOrDefaultAsync(x => x.Id == id);
            Assert.NotNull(created);
            Assert.Equal("Nowy budynek", created.Name);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_False_When_Not_Found()
        {
            await using var dbContext = TestDbContextFactory.Create();
            var service = new BuildingService(dbContext, MapperFactory.Create());

            var updated = await service.UpdateAsync(new UpdateBuildingDto
            {
                Id = 99999,
                Name = "X",
                Address = "Y"
            });

            Assert.False(updated);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(99999)]
        public async Task DeleteAsync_Should_Return_False_When_Building_Not_Found(int notExistingId)
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);
            var service = new BuildingService(dbContext, MapperFactory.Create());

            var deleted = await service.DeleteAsync(notExistingId);

            Assert.False(deleted);
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_When_Building_Has_Rooms()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);
            var service = new BuildingService(dbContext, MapperFactory.Create());

            var buildingId = await dbContext.Buildings
                .Where(x => x.Name == "Test Building A")
                .Select(x => x.Id)
                .FirstAsync();

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(buildingId));
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_For_Empty_Building()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);
            var service = new BuildingService(dbContext, MapperFactory.Create());

            var emptyId = await service.CreateAsync(new CreateBuildingDto
            {
                Name = "No Rooms",
                Address = "None"
            });

            var deleted = await service.DeleteAsync(emptyId);

            Assert.True(deleted);
            Assert.False(await dbContext.Buildings.AnyAsync(x => x.Id == emptyId));
        }
    }
}
