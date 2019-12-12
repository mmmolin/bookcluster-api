using System.Collections.Generic;
using System.Threading.Tasks;
using BookCluster.DataAccess;
using BookCluster.Domain.Interfaces;
using Dapper.Contrib.Extensions;

namespace BookCluster.Repository
{
    //TEntity should be a class, it should have an ID and it should have an constructor.
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IHaveId, new()
    {
        private readonly string connectionString;

        public BaseRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<int> AddAsync(TEntity entity)
        {
            using (var dbContext = new DbContext(connectionString).GetDbContext())
            {
                int id = await dbContext.InsertAsync<TEntity>(entity);
                return id;
            }                
        }

        public async Task DeleteAsync(int id)
        {
            using (var dbContext = new DbContext(connectionString).GetDbContext())
            {
                await dbContext.DeleteAsync(new TEntity { Id = id });
            }
        }

        public async Task<TEntity> FindAsync(int id)
        {
            using (var dbContext = new DbContext(connectionString).GetDbContext())
            {
                return await dbContext.GetAsync<TEntity>(id);
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using (var dbContext = new DbContext(connectionString).GetDbContext())
            {
                return await dbContext.GetAllAsync<TEntity>();
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            using (var dbContext = new DbContext(connectionString).GetDbContext())
            {
                await dbContext.UpdateAsync<TEntity>(entity);
            }
        }
    }
}
