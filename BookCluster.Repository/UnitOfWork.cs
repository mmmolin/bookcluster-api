using BookCluster.DataAccess;
using System.Data;

namespace BookCluster.Repository
{
    public class UnitOfWork
    {
        private readonly string connectionString;
        private BookRepository bookRepository;
        private AuthorRepository authorRepository;
        public UnitOfWork(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public BookRepository BookRepository
        {
            get
            {
                if(bookRepository == null)
                {
                    bookRepository = new BookRepository(connectionString);
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
                    authorRepository = new AuthorRepository(connectionString);
                }
                return authorRepository;
            }
        }
    }
}
