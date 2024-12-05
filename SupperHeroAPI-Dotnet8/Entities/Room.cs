using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class Room
    {
        //    CREATE TABLE Rooms(
        //    RoomID INT IDENTITY(1,1) PRIMARY KEY,
        //RoomNumber NVARCHAR(10) NOT NULL UNIQUE,
        //RoomTypeID INT NOT NULL,
        //Status NVARCHAR(20) CHECK(Status IN ('Available', 'Occupied', 'UnderMaintenance')) DEFAULT 'Available',
        //FOREIGN KEY(RoomTypeID) REFERENCES RoomTypes(RoomTypeID)
        [Key]
        public int IdRoom { get; set; }
        public required string RoomNumber { get; set;}
        public required int RoomTypeID { get; set;}
        public  RoomType? RoomType { get; set;}
        public bool? isActive { get; set; } = true;

        public Status status { get; set; } = Status.Available;
        public ICollection<BookingRoom>? BookingRooms { get; set; }
        public ICollection<Image>? Images { get; set; }
    }   
   public enum Status
    {
        Available, Occupied, UnderMaintenance
    }
}
