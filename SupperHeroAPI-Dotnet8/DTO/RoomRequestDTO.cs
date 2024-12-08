namespace SupperHeroAPI_Dotnet8.DTO
{
    public class RoomRequestDTO
    {
        public int IdRoomType {  get; set; }
        public string NumberRoom { get; set; }
        public ICollection<IFormFile> FileImages { get; set; }
    }
}
