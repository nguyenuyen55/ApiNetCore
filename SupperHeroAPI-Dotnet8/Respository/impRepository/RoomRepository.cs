using Microsoft.EntityFrameworkCore;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Respository.impRepository
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(DataContext context) : base(context)
        {
        }
    }
}
