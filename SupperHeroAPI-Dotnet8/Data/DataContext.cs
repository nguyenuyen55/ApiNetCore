using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) { }
        public DbSet<TokenRefresh> TokenRefreshes { get; set; }
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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.UserId);
                entity.HasIndex(x => x.userName).IsUnique();
            });
            modelBuilder.Entity<RoomType>()
                .HasKey(r => r.Id);
            //table room
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.IdRoom);

                entity.Property(e => e.status)
                      .HasConversion<string>();

                entity.HasOne(r => r.RoomType)
                      .WithMany(rt => rt.Rooms)
                      .HasForeignKey(r => r.RoomTypeID);
            });
            //table booking
            modelBuilder.Entity<Booking>(entity =>
            {
               entity.HasKey(booking => booking.Id);
               entity.Property(e => e.status)
                     .HasConversion<string>();
                entity.HasOne(booking => booking.User)
                    .WithMany(user => user.bookings)
                    .HasForeignKey(booking => booking.UserId);
            });

            // bảng trung gian
            modelBuilder.Entity<BookingRoom>(entity =>
            {
                entity.HasKey(br => new { br.IdBooking, br.IdRoom });

                entity.HasOne(bookingRoom => bookingRoom.Room)
                    .WithMany(room => room.BookingRooms)
                    .HasForeignKey(bookingRoom => bookingRoom.IdRoom);
                entity.HasOne(bookingRoom => bookingRoom.Booking)
                    .WithMany(room => room.BookingRooms)
                    .HasForeignKey(bookingRoom => bookingRoom.IdBooking);
            });


            //Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(payment => payment.Id);
                entity.Property(e => e.paymentMethod)
                      .HasConversion<string>();
                entity.HasOne(Payment => Payment.Booking)
                    .WithOne(booking => booking.Payment)
                    .HasForeignKey<Payment>(payment => payment.BookingID);
            });

            //image
            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(image => image.id);
                entity.HasOne(img => img.Room)
                      .WithMany(room => room.Images)
                      .HasForeignKey(img => img.RoomID);
            });
           //refresh

        }
    }
}
