using BookCluster.DataAccess;
using BookCluster.Domain.Entities;
using BookCluster.Domain.Interfaces;
using Dapper;
using Konscious.Security.Cryptography;
using System;
using System.Linq;
using System.Text;
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
            string sql = "SELECT * FROM Account WHERE Account.UserName = @userName";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                var userResultAsync = await connection.QueryAsync<User>(sql, parameters);
                userResult = userResultAsync.FirstOrDefault();
            }

            if (userResult != null)
            {
                var inputPasswordHash = HashPassword(parameters.password, userResult.Salt);
                var userAccountHash = Convert.FromBase64String(userResult.Hash); // Temporary, store byte[] as varbinary(max) in db.

                userResult = ValidateHash(userAccountHash, inputPasswordHash) ? userResult : null;
            }

            return userResult;
        }

        public async Task<bool> AddUserAsync(string userName, string password)
        {
            var newSalt = Guid.NewGuid().ToString();
            var newHash = HashPassword(password, newSalt);
            var convertedHash = Convert.ToBase64String(newHash);

            bool insertSuccess = false;
            var parameters = new { username = userName, hash = convertedHash, salt = newSalt };
            string sql = "INSERT INTO Account (UserName, Hash, Salt) VALUES (@username, @hash, @salt)";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                var affectedRows = await connection.ExecuteAsync(sql, parameters);
                insertSuccess = affectedRows != 0;
            }

            return insertSuccess;
        }


        public byte[] HashPassword(string password, string salt)
        {
            byte[] newHash = null;
            using (var argon2 = new Argon2i(Encoding.ASCII.GetBytes(password)))
            {
                argon2.Salt = Encoding.ASCII.GetBytes(salt);
                argon2.DegreeOfParallelism = 16;
                argon2.Iterations = 40;
                argon2.MemorySize = 8192;

                newHash = argon2.GetBytes(128);
            }

            return newHash;
        }

        public bool ValidateHash(byte[] userAccountHash, byte[] passwordInput)
        {
            var isValid = userAccountHash.SequenceEqual(passwordInput);
            return isValid;
        }
    }
}
