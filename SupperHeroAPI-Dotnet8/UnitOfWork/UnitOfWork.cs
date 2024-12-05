using Microsoft.EntityFrameworkCore;
using SupperHeroAPI_Dotnet8.Data;
using SupperHeroAPI_Dotnet8.Respository;

namespace SupperHeroAPI_Dotnet8.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork,IDisposable
    {
       private readonly DataContext _dataContext;
        public UnitOfWork(IRoomRepository _roomRepository, DataContext dataContext)
        {
            roomRepository = _roomRepository;
            _dataContext = dataContext;
        }

        public IRoomRepository roomRepository => throw new NotImplementedException();

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public  async Task<int> SaveChangesAsync()
        {
           return await _dataContext.SaveChangesAsync();
        }
    }
}
