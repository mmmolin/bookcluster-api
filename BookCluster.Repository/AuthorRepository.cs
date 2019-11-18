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
    }
}
