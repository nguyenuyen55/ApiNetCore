using SupperHeroAPI_Dotnet8.Entities;

namespace SupperHeroAPI_Dotnet8.Respository
{
    public interface IGenericRepository< T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T t);
        Task UpdateAsync(T t);
        Task DeleteAsync(int id);
    }
}
