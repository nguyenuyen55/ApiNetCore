using SupperHeroAPI_Dotnet8.Respository;

namespace SupperHeroAPI_Dotnet8.UnitOfWork
{
    public interface IUnitOfWork 
    {
       public IRoomRepository roomRepository { get;  }
        Task<int> SaveChangesAsync();
    }
}
