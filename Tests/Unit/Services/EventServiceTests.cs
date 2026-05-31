using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.DataModels;
using Services.DTO.Event;
using Services.Services;
using Tests.TestInfrastructure;

namespace Tests.Unit.Services
{
	public class EventServiceTests
	{
		[Fact]
		public async Task CreateAsync_Should_Throw_When_EventType_Does_Not_Exit()
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new EventService(dbContext, MapperFactory.Create());

			var eventCreated = new CreateEventDto
			{
				Title = "Test Event",
				Description = "Test Description",
				ParticipantsLimit = 10,
				IsPublic = true,
				CreatedAt = DateTime.UtcNow,
				EventTypeId = 99999
			};

			await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(eventCreated));
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task CreateAsync_Should_Throw_When_Participants_Invalid(int participantsLimit)
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new EventService(dbContext, MapperFactory.Create());

			var eventTypeId = await dbContext.EventTypes
				.Where(x => x.Name == "Lecture")
				.Select(x => x.Id)
				.FirstAsync();

			var eventCreated = new CreateEventDto
			{
				Title = "Test Event",
				Description = "Test Description",
				ParticipantsLimit = participantsLimit,
				IsPublic = true,
				CreatedAt = DateTime.UtcNow,
				EventTypeId = eventTypeId
			};

			await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(eventCreated));
		}

		[Fact]
		public async Task GetPublicEventsAsync_Should_Return_Only_Public_Events()
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new EventService(dbContext, MapperFactory.Create());

			var result = await service.GetPublicEventsAsync();

			Assert.All(result, item => Assert.True(item.IsPublic));
		}

		[Fact]
		public async Task UpdateAsync_Should_Return_False_When_Not_Found()
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new EventService(dbContext, MapperFactory.Create());

			var eventTypeId = await dbContext.EventTypes
				.Where(x => x.Name == "Lecture")
				.Select(x => x.Id)
				.FirstAsync();

			var updated = await service.UpdateAsync(new UpdateEventDto
			{
				Id = 99999,
				Title = "Test Event",
				Description = "Test Description",
				ParticipantsLimit = 10,
				IsPublic = true,
				CreatedAt = DateTime.UtcNow,
				EventTypeId = eventTypeId
			});

			Assert.False(updated);
		}

		[Fact]
		public async Task DeleteAsync_Should_Throw_When_Event_Has_Reservations()
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new EventService(dbContext, MapperFactory.Create());

			var eventId = await dbContext.Events
				.Where(x => x.Title == "Public Test Event")
				.Select(x => x.Id)
				.FirstAsync();

			await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(eventId));
		}
	}
}

