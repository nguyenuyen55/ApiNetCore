using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Principal;
using System;
using System.Text.Json.Serialization;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class Booking
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        //public int RoomId { get; set; }
        //public Room? Room { get; set; }
        public ICollection<BookingRoom>? BookingRooms { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public statusBooking status { get; set; } = statusBooking.Pending;
        public DateTime? CreateAt { get; set; } = DateTime.Now;
        public Payment? Payment { get; set; }
    }
    public enum statusBooking
    {
        Pending,
        Confirmed,
        Cancelled
    }
   
}
