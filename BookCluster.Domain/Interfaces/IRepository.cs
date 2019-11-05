using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookCluster.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);


        Task DeleteAsync(int id);


        Task<TEntity> FindAsync(int id);


        Task<IEnumerable<TEntity>> GetAllAsync();


        Task UpdateAsync(TEntity entity);
    }
}
