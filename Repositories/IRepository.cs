using System.Threading.Tasks;

namespace Game2048.Repositories
{
    public interface IRepository<T>
    {
        Task<int> AddAsync(T data);
        Task UpdateAsync(T data);
        Task DeleteAsync(int id);
    }
}