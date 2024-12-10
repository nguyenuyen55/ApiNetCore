using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.DTO.Booking
{
    public class BookingInsert
    {

        public Guid UserId { get; set; }

        public ICollection<BookingRoomDTO>? BookingRooms { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double Amount { get; set; }
    }
}
