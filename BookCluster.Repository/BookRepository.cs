using BookCluster.Domain.Entities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BookCluster.Repository
{
    public class BookRepository : BaseRepository<Book>
    {
        private IDbConnection dbContext;
        public BookRepository(IDbConnection dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Book>> GetAuthorRelatedBooksAsync(int authorId)
        {
            List<Book> bookResult = null;
            var parameters = new { id = authorId };
            string sql = "SELECT * FROM Book WHERE Book.AuthorId = @id";
            using (var connection = dbContext)
            {
                var bookResultAsync = await connection.QueryAsync<Book>(sql, parameters);
                bookResult = bookResultAsync.ToList();
            }

            return bookResult;
        }
    }
}
