using BookCluster.DataAccess;
using BookCluster.Domain.Entities;
using BookCluster.Domain.Interfaces;
using Dapper;
using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
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

        public async Task<ICollection<Book>> GetUserBookAsync(string userId, string bookId)
        {
            ICollection<Book> userBookResult = null;
            var parameters = new { userid = userId, bookid = bookId };
            var sql = "SELECT * FROM BookAccount WHERE BookAccount.AccountID = @userid AND BookAccount.BookID = @bookid";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                userBookResult = await connection.QueryAsync<Book>(sql, parameters) as ICollection<Book>;
            }

            return userBookResult;
        }

        public async Task<IEnumerable<Book>> GetUserBooksAsync(string userId)
        {
            IEnumerable<Book> userBookResults = null;
            var parameters = new { userid = userId };
            string sql = "SELECT Book.ID, Book.Title FROM Book INNER JOIN BookAccount ON Book.ID = BookAccount.BookID WHERE BookAccount.AccountID = @userid";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                userBookResults = await connection.QueryAsync<Book>(sql, parameters);
            }

            return userBookResults;
        }

        public async Task<bool> AddBookToUser(string userId, string bookId)
        {
            bool insertSuccess = false; 
            var parameters = new { userid = userId, bookid = bookId };
            var sql = "INSERT INTO BookAccount (BookID, AccountID) VALUES (@bookid, @userid)";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                var rowsAffected = await connection.ExecuteAsync(sql, parameters);
                if(rowsAffected == 1)
                {
                    insertSuccess = true;
                }
            }

            return insertSuccess;
        }

        public async Task<bool> RemoveBookFromUser(string userId, string bookId)
        {
            bool deleteSuccess;
            var parameters = new { userid = userId, bookid = bookId };
            var sql = "DELETE FROM BookAccount WHERE BookAccount.AccountID = @userid AND BookAccount.BookID = @bookid";
            using (var connection = new DbContext(connectionString).GetDbContext())
            {
                var affectedRows = await connection.ExecuteAsync(sql, parameters);
                deleteSuccess = affectedRows == 1;
            }

            return deleteSuccess;
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
