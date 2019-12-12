using BookCluster.Domain.Entities;
using System.Data;

namespace BookCluster.Repository
{
    public class AuthorRepository : BaseRepository<Author>
    {
        private readonly string connectionString;

        public AuthorRepository(string connectionString) : base(connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
