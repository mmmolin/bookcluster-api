using BookCluster.Domain.Entities;
using Dapper;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BookCluster.Repository
{
    public class AuthorRepository : BaseRepository<Author>
    {
        private IDbConnection dbContext;
        public AuthorRepository(IDbConnection dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Author>GetAuthorAndBooksAsync(int id)
        {
            Author author = null;
            var parameters = new { Id = id };
            string sql = "SELECT * FROM Author WHERE Author.ID = @Id; SELECT * FROM BOOK WHERE Book.AuthorID = @Id";
            using (var multi = await dbContext.QueryMultipleAsync(sql, parameters))
            {
                var authorAsyncResult = await multi.ReadAsync<Author>();
                var authorResult = authorAsyncResult.FirstOrDefault();
                if (authorResult != null)
                {
                    var booksAsyncResult = await multi.ReadAsync<Book>();
                    author = authorResult;
                    author.Books = booksAsyncResult.ToList();
                }
            }
            return author;
        }
    }
}
