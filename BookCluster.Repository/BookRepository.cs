using BookCluster.DataAccess;
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
        private readonly string connectionString;

        public BookRepository(string connectionString) : base(connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<Book>> GetAuthorRelatedBooksAsync(int authorId)
        {
            List<Book> bookResult = null;
            var parameters = new { id = authorId };
            string sql = "SELECT * FROM Book WHERE Book.AuthorId = @id";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                var bookResultAsync = await connection.QueryAsync<Book>(sql, parameters);
                bookResult = bookResultAsync.ToList();
            }

            return bookResult;
        }

        public async Task<bool> RemoveAllAuthorRelatedBooks(int authorId)
        {
            var parameters = new { id = authorId };
            string sql = "DELETE FROM Book WHERE AuthorId = @id";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                int rowsAffected = await connection.ExecuteAsync(sql, parameters);
                return rowsAffected > 0;
            }
        }
    }
}
