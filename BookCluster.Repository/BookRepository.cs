using BookCluster.Domain.Entities;
using System.Data;

namespace BookCluster.Repository
{
    public class BookRepository : BaseRepository<Book>
    {
        private IDbConnection dbContext;
        public BookRepository(IDbConnection dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }


    }
}
