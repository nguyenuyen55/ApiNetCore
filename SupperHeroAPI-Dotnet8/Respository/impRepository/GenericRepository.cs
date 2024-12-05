
using SupperHeroAPI_Dotnet8.Data;

namespace SupperHeroAPI_Dotnet8.Respository.impRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        DataContext? _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }
        public  async Task AddAsync(T t)
        {
          await  _context!.Set<T>().AddAsync(t);
        }

        public async Task DeleteAsync(int id)
        {
           var entity= await _context.Set<T>().FindAsync(id);
            if(entity != null)
            {
                _context!.Set<T>().Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return  _context!.Set<T>().ToList();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context!.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T t)
        {
            _context!.Set<T>().Update(t);
        }
    }
}
