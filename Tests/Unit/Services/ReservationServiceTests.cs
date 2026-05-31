using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.DataModels;
using Services.DTO.Reservation;
using Services.Services;
using Tests.TestInfrastructure;

namespace Tests.Unit.Services
{
	public class ReservationServiceTests
	{
		[Theory]
		[InlineData(-30)]
		[InlineData(0)]
		public async Task CreateAsync_Should_Throw_When_Event_Has_Reservations(int timeDifference)
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new ReservationService(dbContext, MapperFactory.Create());

			var startTime = new DateTime(2026, 5, 31, 10, 30, 0);
			var endTime = startTime.AddMinutes(timeDifference);

			var roomId = await dbContext.Rooms
				.Where(x => x.Name == "A-101")
				.Select(x => x.Id)
				.FirstAsync();

			var eventId = await dbContext.Events
				.Where(x => x.Title == "Public Test Event")
				.Select(x => x.Id)
				.FirstAsync();

			await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new CreateReservationDto
			{
				StartTime = startTime,
				EndTime = endTime,
				Status = Model.DataModels.ReservationStatus.Approved,
				Notes = "Test Notes",
				RoomId = roomId,
				EventId = eventId
			}));
		}

		[Fact]
		public async Task CreateAsync_Should_Throw_When_Room_Incative()
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new ReservationService(dbContext, MapperFactory.Create());

			var inactiveRoomId = await dbContext.Rooms
				.Where(x => x.Name == "A-201")
				.Select(x => x.Id)
				.FirstAsync();

			var eventId = await dbContext.Events
				.Where(x => x.Title == "Public Test Event")
				.Select(x => x.Id)
				.FirstAsync();

			await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new CreateReservationDto
			{
				StartTime = new DateTime(2026, 5, 31, 10, 30, 0),
				EndTime = new DateTime(2026, 5, 31, 12, 30, 0),
				Status = Model.DataModels.ReservationStatus.Approved,
				Notes = "Test Notes",
				RoomId = inactiveRoomId,
				EventId = eventId
			}));
		}

		[Fact]
		public async Task CreateAsync_Should_Throw_When_Time_Conflict_Exists()
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new ReservationService(dbContext, MapperFactory.Create());

			var roomId = await dbContext.Rooms
				.Where(x => x.Name == "A-101")
				.Select(x => x.Id)
				.FirstAsync();

			var eventId = await dbContext.Events
				.Where(x => x.Title == "Public Test Event")
				.Select(x => x.Id)
				.FirstAsync();

			await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new CreateReservationDto
			{
				StartTime = new DateTime(2026, 5, 31, 10, 30, 0),
				EndTime = new DateTime(2026, 5, 31, 9, 30, 0),
				Status = Model.DataModels.ReservationStatus.Cancelled,
				Notes = "Test Notes",
				RoomId = roomId,
				EventId = eventId
			}));
		}

		[Fact]
		public async Task ChangeStatusAsync_Should_Update_Async()
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new ReservationService(dbContext, MapperFactory.Create());

			var roomId = await dbContext.Rooms
				.Where(x => x.Name == "A-101")
				.Select(x => x.Id)
				.FirstAsync();

			var eventId = await dbContext.Events
				.Where(x => x.Title == "Public Test Event")
				.Select(x => x.Id)
				.FirstAsync();

			var reservationId = await dbContext.Reservations
				.Where(x => x.RoomId == roomId)
				.Select(x => x.Id)
				.FirstAsync();

			var result = await service.UpdateAsync(new UpdateReservationDto
			{
				Id = reservationId,
				StartTime = DateTime.UtcNow.AddDays(2),
				EndTime = DateTime.UtcNow.AddDays(2).AddHours(3),
				Status = Model.DataModels.ReservationStatus.Cancelled,
				Notes = "Test Notes",
				RoomId = roomId,
				EventId = eventId
			});

			Assert.True(result);
		}

		[Fact]
		public async Task ChangeStatusAsync_Should_Return_False_When_Not_Found()
		{
			await using var dbContext = TestDbContextFactory.Create();
			await TestDataSeeder.SeedAsync(dbContext);

			var service = new ReservationService(dbContext, MapperFactory.Create());

			var roomId = await dbContext.Rooms
				.Where(x => x.Name == "A-101")
				.Select(x => x.Id)
				.FirstAsync();

			var eventId = await dbContext.Events
				.Where(x => x.Title == "Public Test Event")
				.Select(x => x.Id)
				.FirstAsync();

			var reservationId = await dbContext.Reservations
				.Where(x => x.RoomId == roomId)
				.Select(x => x.Id)
				.FirstAsync();

			var result = await service.UpdateAsync(new UpdateReservationDto
			{
				Id = 9999,
				StartTime = DateTime.UtcNow.AddDays(10),
				EndTime = DateTime.UtcNow.AddDays(10).AddHours(2),
				Status = Model.DataModels.ReservationStatus.Cancelled,
				Notes = "Test Notes",
				RoomId = roomId,
				EventId = eventId
			});

			Assert.False(result);
		}
	}
}
