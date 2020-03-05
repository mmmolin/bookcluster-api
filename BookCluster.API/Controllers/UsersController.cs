using AutoMapper;
using BookCluster.API.Models;
using BookCluster.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

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
                if (insertSuccess)
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
        [HttpGet("{Books}")]
        public async Task<ActionResult<List<Book>>> GetUserRelatedBooksAsync()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var entities = await unitOfWork.UserRepository.GetUserBooksAsync(userId);
                if(entities != null)
                {
                    var books = mapper.Map<Models.Book[]>(entities);
                    return Ok(books);
                }

                return Ok(); // StatusCode 204 or 404?
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Database Failure");
            }
        }

        [HttpPost("{Books}")]
        public async Task<ActionResult<bool>> AddBookToUserAsync([FromBody] string bookId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var insertSuccess = await unitOfWork.UserRepository.AddBookToUser(userId, bookId);
                if (insertSuccess)
                {
                    return Ok();
                }

                return Conflict();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Database Failure");
            }
        }


        // Delete User Books


    }
}