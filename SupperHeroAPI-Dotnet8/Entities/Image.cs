using System.Text.Json.Serialization;

namespace SupperHeroAPI_Dotnet8.Entities
{
    public class Image
    {

        //Copy code
        //CREATE TABLE Images(
        //    ImageID INT IDENTITY(1,1) PRIMARY KEY,
        //    RoomID INT NOT NULL,
        //    ImagePath NVARCHAR(255) NOT NULL,
        //    UploadedAt DATETIME DEFAULT GETDATE(),
        //    FOREIGN KEY(RoomID) REFERENCES Rooms(RoomID)
        //);

        public int id { get; set; }
        public int RoomID { get; set; }
        public string? NameImage { get; set; }
        [JsonIgnore]
        public Room? Room { get; set; }
        public DateTime dateTime { get; set; }= DateTime.Now;


    }

}
