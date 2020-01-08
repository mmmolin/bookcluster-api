using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookCluster.Domain.Entities;

namespace BookCluster.IdentityServer.Configuration
{
    public class UserValidator : IUserValidator
    {
        public Task<User> FindByUsernameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserValidator
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<User> FindByUsernameAsync(string userName);
        //Task<User> FindByExternalProvider(string provider, string userId);
        //Task<User> AutoProvisionUserASync(string provider, string userId, IEnumerable<Claim> claims);
    }
}
