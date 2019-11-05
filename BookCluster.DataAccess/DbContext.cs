using System.Data;
using System.Data.SqlClient;

namespace BookCluster.DataAccess
{
    public class DbContext
    {
        private readonly IDbConnection dbContext;

        public DbContext(string connectionString)
        {
            this.dbContext = new SqlConnection(connectionString);
        }

        public IDbConnection GetDbContext()
        {
            return dbContext;
        }
    }
}
