using SupperHeroAPI_Dotnet8.Respository;

namespace SupperHeroAPI_Dotnet8.UnitOfWork
{
    public interface IUnitOfWork 
    {
        public IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
