using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Service.interfaces
{
    public interface IUserServices
    {
        Task<ApiResponse<User>> InsertRoom(UserInsert userInsert);
    }
}
