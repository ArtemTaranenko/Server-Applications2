using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Services.Services;
using Services.DTO.EventType;
using Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;

namespace Tests.Unit.Services
{
    public class EventTypeServiceTests
    {
        [Fact]
        public async Task GetAllAsync_Should_Return_Seeded_EventTypes()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EventTypeService(dbContext, MapperFactory.Create());

            var result = await service.GetAllAsync();

            Assert.True(result.Count() >= 2);
            Assert.Contains(result, x => x.Name == "Lecture");
            Assert.Contains(result, x => x.Name == "Workshop");
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EventTypeService(dbContext, MapperFactory.Create());

            var result = await service.GetByIdAsync(999999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_EventType()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EventTypeService(dbContext, MapperFactory.Create());

            var id = await service.CreateAsync(new CreateEventTypeDto
            {
                Name = "Test EventType",
                Description = "Test Description"
            });

            var created = await dbContext.EventTypes.FirstOrDefaultAsync(x => x.Id == id);

            Assert.NotNull(created);
            Assert.Equal("Test EventType", created.Name);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_False_When_Not_Found()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EventTypeService(dbContext, MapperFactory.Create());

            var updated = await service.UpdateAsync(new UpdateEventTypeDto
            {
                Id = 999999,
                Name = "Not Found",
                Description = "Also not found"
            });

            Assert.False(updated);
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Found()
        {
            await using var dbContext = TestDbContextFactory.Create();
            await TestDataSeeder.SeedAsync(dbContext);

            var service = new EventTypeService(dbContext, MapperFactory.Create());

            var deleted = await service.DeleteAsync(99999);

            Assert.False(deleted);
        }
    }
}
