
using Microsoft.EntityFrameworkCore;
using SupperHeroAPI_Dotnet8.Data;
using System.Linq.Expressions;

namespace SupperHeroAPI_Dotnet8.Respository.impRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        DataContext? _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }
        public  async Task<T> AddAsync(T t)
        {
        var tEntiry=await  _context!.Set<T>().AddAsync(t);
            return tEntiry.Entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> t)
        {
          if(t != null)
            {
                foreach (var item in t)
                {
                    await _context!.Set<T>().AddAsync(item);
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
           var entity= await _context.Set<T>().FindAsync(id);
            if(entity != null)
            {
                _context!.Set<T>().Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,params Expression<Func<T, object>>[] inclues)
        {
            IQueryable<T> query =_context.Set<T>();
            foreach (var inclue in inclues)
            {
                query=query.Include(inclue);
            }
            //filter
            if(filter != null)
            {
                query.Where(filter);
            }
           return  await query.ToListAsync();
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
