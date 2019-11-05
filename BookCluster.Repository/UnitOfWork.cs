using BookCluster.DataAccess;
using System.Data;

namespace BookCluster.Repository
{
    public class UnitOfWork
    {
        private readonly IDbConnection dbContext;
        private BookRepository bookRepository;
        private AuthorRepository authorRepository;
        public UnitOfWork(string connectionString)
        {
            this.dbContext = new DbContext(connectionString).GetDbContext();
        }

        public BookRepository BookRepository
        {
            get
            {
                if(bookRepository == null)
                {
                    bookRepository = new BookRepository(dbContext);
                }
                return bookRepository;
            }
        }

        public AuthorRepository AuthorRepository
        {
            get
            {
                if(authorRepository == null)
                {
                    authorRepository = new AuthorRepository(dbContext);
                }
                return authorRepository;
            }
        }
    }
}
