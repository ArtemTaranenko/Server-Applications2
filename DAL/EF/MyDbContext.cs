using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Microsoft.EntityFrameworkCore;
using Model.DataModels;



namespace DAL.EF
{
    public class MyDbContext : DbContext
    {

        public virtual DbSet<EventType> EventTypes { get; set; } = null!;
        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<Equipment> Equipments { get; set; } = null!;
        public virtual DbSet<RoomEquipment> RoomEquipments { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<Event> Events { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Reservation>()
                .Property(x => x.Status)
                .HasConversion<string>();
            modelBuilder.Entity<RoomEquipment>()
                .HasIndex(x => new { x.RoomId, x.EquipmentId })
                .IsUnique();
        }
    }
}
