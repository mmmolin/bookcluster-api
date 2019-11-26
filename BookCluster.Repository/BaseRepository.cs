using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using BookCluster.Domain.Interfaces;
using Dapper.Contrib.Extensions;

namespace BookCluster.Repository
{
    //TEntity should be a class, it should have an ID and it should have an constructor.
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IHaveId, new()
    {
        private IDbConnection dbContext;
        public BaseRepository(IDbConnection dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddAsync(TEntity entity)
        {
            int id = await dbContext.InsertAsync<TEntity>(entity);
            return id;
        }

        public async Task DeleteAsync(int id)
        {
            await dbContext.DeleteAsync(new TEntity { Id = id });
        }

        public async Task<TEntity> FindAsync(int id)
        {
            return await dbContext.GetAsync<TEntity>(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbContext.GetAllAsync<TEntity>();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await dbContext.UpdateAsync<TEntity>(entity);
        }
    }
}
