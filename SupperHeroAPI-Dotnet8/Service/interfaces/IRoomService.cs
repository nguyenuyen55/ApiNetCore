using SupperHeroAPI_Dotnet8.DTO;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Service.interfaces
{
    public interface IRoomService
    {
        Task<ApiResponse<Room>> getRoomById(int id);
        Task<ApiResponse<IEnumerable<Room>>> getRoomAll(string? search);

        Task<ApiResponse<Room>> InsertRoom(RoomRequestDTO roomRequest);
        Task<ApiResponse<bool>> ChangeStatusRoom(int idRoom,string status);
        Task<ApiResponse<Room>> UpdateRoom(int idRoom , RoomRequestDTO roomRequest);
        Task<ApiResponse<bool>> DeleteRoom(int idRoom );
    }
}
