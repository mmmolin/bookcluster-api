using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookCluster.API.Models;
using BookCluster.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookCluster.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UnitOfWork unitOfWork;
        private IMapper mapper;

        public UsersController(IOptions<Option> option, IMapper mapper)
        {
            this.unitOfWork = new UnitOfWork(option.Value.ConnectionString);
            this.mapper = mapper;
        }

        // Get User

        // Register User
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> AddUserAsync([FromBody] User user)
        {
            try
            {
                var insertSuccess = await unitOfWork.UserRepository.AddUserAsync(user.UserName, user.Password);
                if(insertSuccess)
                {
                    return Created("test", user.UserName);
                }

                return UnprocessableEntity();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Database Failure");
            }
        }

        // Update UserInformation

        // Get User Books

        // Add User Books

        // Delete User Books


    }
}