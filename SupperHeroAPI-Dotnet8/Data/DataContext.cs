using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Image> images { get; set; }
        public DbSet<BookingRoom> bookingRooms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(x=>x.UserId);
            modelBuilder.Entity<RoomType>()
                .HasKey(r => r.Id);
            //table room
            modelBuilder.Entity<Room>().HasKey(r => r.IdRoom);

            modelBuilder.Entity<Room>()
                .Property(e=>e.status)
                .HasConversion<string>();
            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomType)
                .WithMany(rt=>rt.Rooms)
                .HasForeignKey(r=>r.RoomTypeID);
       
            //table booking
        
            modelBuilder.Entity<Booking>().HasKey(booking => booking.Id);
            modelBuilder.Entity<Booking>()
             .Property(e => e.status)
             .HasConversion<string>();
            modelBuilder.Entity<Booking>()
                .HasOne(booking => booking.User)
                .WithMany(user => user.bookings)
                .HasForeignKey(booking => booking.UserId);

            // bảng trung gian
            modelBuilder.Entity<BookingRoom>().HasKey(br => new { br.IdBooking, br.IdRoom });

            modelBuilder.Entity<BookingRoom>()
                .HasOne(bookingRoom => bookingRoom.Room)
                .WithMany(room => room.BookingRooms)
                .HasForeignKey(bookingRoom => bookingRoom.IdRoom);

            modelBuilder.Entity<BookingRoom>()
                .HasOne(bookingRoom => bookingRoom.Booking)
                .WithMany(room => room.BookingRooms)
                .HasForeignKey(bookingRoom => bookingRoom.IdBooking);

            //Payment
            modelBuilder.Entity<Payment>().HasKey(payment => payment.Id);
            modelBuilder.Entity<Payment>()
               .Property(e => e.paymentMethod)
               .HasConversion<string>();
            modelBuilder.Entity<Payment>()
                .HasOne(Payment => Payment.Booking)
                .WithOne(booking => booking.Payment)
                .HasForeignKey<Payment>(payment=>payment.BookingID);
            //image
            modelBuilder.Entity<Image>().HasKey(image => image.id);
            modelBuilder.Entity<Image>()
               .HasOne(img=>img.Room)
               .WithMany(room => room.Images)
               .HasForeignKey(img=>img.RoomID);
        }
    }
}
