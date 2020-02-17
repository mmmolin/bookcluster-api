using BookCluster.DataAccess;
using System.Data;

namespace BookCluster.Repository
{
    public class UnitOfWork
    {
        private readonly string connectionString;
        private BookRepository bookRepository;
        private AuthorRepository authorRepository;
        private UserRepository userRepository;

        public UnitOfWork(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // When instantiating UnitOfWork, these getters make sure to only instatiate the needed repository.

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

        public UserRepository UserRepository
        {
            get
            {
                if(userRepository == null)
                {
                    userRepository = new UserRepository(connectionString);
                }
                return userRepository;
            }
        }
    }
}
