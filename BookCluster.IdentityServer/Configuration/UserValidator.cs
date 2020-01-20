using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookCluster.Domain.Entities;
using BookCluster.Domain.Interfaces;
using BookCluster.Repository;

namespace BookCluster.IdentityServer.Configuration
{
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository userRepository;

        public UserValidator(IUserRepository repository)
        {
            this.userRepository = repository;
        }

        public Task<User> FindByUsernameAsync(string userName)
        {
            return userRepository.FindUserAsync(userName);
        }

        public async Task<bool> ValidateCredentialsAsync(string userName, string password)
        {
            var user = await userRepository.GetUserAsync(userName, password);
            return user != null;
        }
    }

    public interface IUserValidator
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
        Task<User> FindByUsernameAsync(string userName);
    }
}
