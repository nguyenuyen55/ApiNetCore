using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace SupperHeroAPI_Dotnet8.Entities
{

    public class RoomType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double PricePerNight { get; set; }
      public  ICollection<Room>? Rooms { get; set;}

    }
}
