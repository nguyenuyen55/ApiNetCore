using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text.Json.Serialization;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class Room
    {

        [Key]
        public int IdRoom { get; set; }
        public  string RoomNumber { get; set;}
        public  int RoomTypeID { get; set;}
        public  RoomType? RoomType { get; set;}
        public bool? isActive { get; set; } = true;

        public Status status { get; set; } = Status.Available;
        [JsonIgnore]
        public ICollection<BookingRoom>? BookingRooms { get; set; }
        public ICollection<Image>? Images { get; set; }
       
    }   
   public enum Status
    {
        Available, Occupied, UnderMaintenance
    }
}
