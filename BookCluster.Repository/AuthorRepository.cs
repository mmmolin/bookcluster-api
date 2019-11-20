using BookCluster.Domain.Entities;
using System.Data;

namespace BookCluster.Repository
{
    public class AuthorRepository : BaseRepository<Author>
    {
        private IDbConnection dbContext;
        public AuthorRepository(IDbConnection dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
