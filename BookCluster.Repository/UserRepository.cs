using BookCluster.DataAccess;
using BookCluster.Domain.Entities;
using Dapper;
using System.Threading.Tasks;

namespace BookCluster.Repository
{
    class UserRepository : BaseRepository<User>
    {
        private readonly string connectionString;

        public UserRepository(string connectionString) : base(connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<User> GetUserAsync(string userName, string passwordHash)
        {
            User userResult = null;
            var parameters = new { username = userName, passwordhash = passwordHash };
            string sql = "SELECT * FROM Account WHERE Account.UserName = @username AND Account.PasswordHash = @password";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                var userResultAsync = await connection.QueryAsync<User>(sql, parameters);
                userResult = userResultAsync as User;
            }

            return userResult;
        }
    }
}
