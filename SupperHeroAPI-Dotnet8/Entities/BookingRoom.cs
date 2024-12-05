namespace SupperHeroAPI_Dotnet8.Entities
{
    public class BookingRoom
    {
        public int IdRoom { get; set; }
        public Room? Room { get; set; }
        public Guid IdBooking { get; set; }

        public Booking? Booking { get; set; }
    }
}
