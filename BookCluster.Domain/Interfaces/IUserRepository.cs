using BookCluster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookCluster.Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> FindUserAsync(string userName);


        public Task<User> GetUserAsync(string userName, string passWord);
        
    }
}
