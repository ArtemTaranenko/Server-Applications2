using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Model.DataModels;

namespace Tests.TestInfrastructure
{
    internal static class TestDataSeeder
    {
        public static async Task SeedAsync(MyDbContext dbContext)
        {
            await dbContext.Database.EnsureCreatedAsync();

            if (await dbContext.Buildings.AnyAsync())
                return;

            var buildingA = new Building
            {
                Name = "Test Building A",
                Address = "Test Street 1",
                Description = "Building A"
            };

            var buildingB = new Building
            {
                Name = "Test Building B",
                Address = "Test Street 2",
                Description = "Building B"
            };

            var buildingC = new Building
            {
                Name = "Test Building C",
                Address = "Test Street 3",
                Description = "Building C"
            };

            dbContext.Buildings.AddRange(buildingA, buildingB, buildingC);
            await dbContext.SaveChangesAsync();

            var roomA101 = new Room
            {
                Name = "A-101",
                Capacity = 40,
                Floor = 1,
                IsActive = true,
                BuildingId = buildingA.Id
            };

            var roomA201 = new Room
            {
                Name = "A-201",
                Capacity = 20,
                Floor = 2,
                IsActive = false,
                BuildingId = buildingA.Id
            };

            var roomB10 = new Room
            {
                Name = "B-10",
                Capacity = 15,
                Floor = 1,
                IsActive = true,
                BuildingId = buildingB.Id
            };

            dbContext.Rooms.AddRange(roomA101, roomA201, roomB10);

            var projector = new Equipment
            {
                Name = "Projector",
                Description = "Projector for tests",
                IsMobile = false
            };

            var laptop = new Equipment
            {
                Name = "Laptop",
                Description = "Laptop for tests",
                IsMobile = true
            };

            var speakers = new Equipment
            {
                Name = "Speakers",
                Description = "Speakers for tests",
                IsMobile = false
            };

            dbContext.Equipments.AddRange(projector, laptop, speakers);

            var lectureType = new EventType
            {
                Name = "Lecture",
                Description = "Lecture type"
            };

            var workshopType = new EventType
            {
                Name = "Workshop",
                Description = "Workshop type"
            };

            dbContext.EventTypes.AddRange(lectureType, workshopType);
            await dbContext.SaveChangesAsync();

            var publicEvent = new Event
            {
                Title = "Public Test Event",
                Description = "Seeded public event",
                ParticipantsLimit = 30,
                IsPublic = true,
                EventTypeId = lectureType.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            };

            var privateEvent = new Event
            {
                Title = "Private Test Event",
                Description = "Seeded private event",
                ParticipantsLimit = 10,
                IsPublic = false,
                EventTypeId = workshopType.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            };

            dbContext.Events.AddRange(publicEvent, privateEvent);
            await dbContext.SaveChangesAsync();

            dbContext.RoomEquipments.AddRange(
                new RoomEquipment
                {
                    RoomId = roomA101.Id,
                    EquipmentId = projector.Id,
                    Quantity = 1
                },
                new RoomEquipment
                {
                    RoomId = roomB10.Id,
                    EquipmentId = speakers.Id,
                    Quantity = 2
                });

            dbContext.Reservations.AddRange(
                new Reservation
                {
                    RoomId = roomA101.Id,
                    EventId = publicEvent.Id,
                    StartTime = DateTime.UtcNow.AddDays(2),
                    EndTime = DateTime.UtcNow.AddDays(2).AddHours(2),
                    Status = ReservationStatus.Approved,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Notes = "Seeded reservation"
                },
                new Reservation
                {
                    RoomId = roomB10.Id,
                    EventId = privateEvent.Id,
                    StartTime = DateTime.UtcNow.AddDays(3),
                    EndTime = DateTime.UtcNow.AddDays(3).AddHours(1),
                    Status = ReservationStatus.Cancelled,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    Notes = "Cancelled reservation"
                });

            await dbContext.SaveChangesAsync();
        }
    }
}
