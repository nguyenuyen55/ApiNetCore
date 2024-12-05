using Microsoft.EntityFrameworkCore;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.Respository;
using SupperHeroAPI_Dotnet8.Respository.impRepository;

namespace SupperHeroAPI_Dotnet8.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
       private readonly DataContext _dataContext;
        public UnitOfWork( DataContext dataContext)
        {
       
            _dataContext = dataContext;
        }

        
        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
           return new GenericRepository<T>(_dataContext);
        }

        public  async Task<int> SaveChangesAsync()
        {
           return await _dataContext.SaveChangesAsync();
        }
    }
}
