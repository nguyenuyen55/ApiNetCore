using SupperHeroAPI_Dotnet8.Entities;
using System.Linq.Expressions;

namespace SupperHeroAPI_Dotnet8.Respository
{
    public interface IGenericRepository< T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter=null , params Expression<Func<T, object>>[] inclues);
        Task<T> AddAsync(T t);
        Task AddRangeAsync(IEnumerable<T> t);
        Task UpdateAsync(T t);
        Task DeleteAsync(int id);
    }
}
