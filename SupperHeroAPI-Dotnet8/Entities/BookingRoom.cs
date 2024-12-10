using System.Text.Json.Serialization;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class BookingRoom
    {
        public int IdRoom { get; set; }
        public Room? Room { get; set; }
        public string TypeBed { get; set; }

        public Guid IdBooking { get; set; }
   
        public Booking? Booking { get; set; }
    }
}
