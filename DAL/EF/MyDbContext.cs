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

        public virtual ICollection<EventType>? EventTypes {  get; set; }
        public virtual ICollection<Building>? Buildings { get; set; }
        public virtual ICollection<Equipment>? Equipments { get; set; }
        public virtual ICollection<RoomEquipment>? RoomEquipments { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<Event>? Events { get; set; }
        public virtual ICollection<Room>? Rooms { get; set; }

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
