using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.DTO
{
    public class UserInsert
    {
        public required string userName { get; set; }
        public required string passWord { get; set; }
        public required string fullName { get; set; }
        public required string email { get; set; }
        public required string phoneNumber { get; set; }

    }
}
