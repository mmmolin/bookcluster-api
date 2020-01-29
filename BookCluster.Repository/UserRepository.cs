using BookCluster.DataAccess;
using BookCluster.Domain.Entities;
using BookCluster.Domain.Interfaces;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace BookCluster.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(string connectionString) : base(connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<User> FindUserAsync(string userName)
        {
            User userResult = null;
            var parameters = new { username = userName };
            string sql = "SELECT * FROM Account WHERE Account.UserName = @userName";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                var userResultAsync = await connection.QueryAsync<User>(sql, parameters);
                userResult = userResultAsync.FirstOrDefault();
                // Check for null!
            }

                return userResult;
        }

        public async Task<User> GetUserAsync(string userName, string passWord)
        {
            User userResult = null;
            var parameters = new { username = userName, password = passWord };
            string sql = "SELECT * FROM Account WHERE Account.UserName = @username AND Account.Password = @password";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                var userResultAsync = await connection.QueryAsync<User>(sql, parameters);
                userResult = userResultAsync.FirstOrDefault();
                // Check for null!
            }

            return userResult;
        }
    }
}
