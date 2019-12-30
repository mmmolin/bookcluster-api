using System;
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
    [ApiController] // test, does it work?
    public class AuthorsController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthorsController(IOptions<Option> options, IMapper mapper)
        {
            this.unitOfWork = new UnitOfWork(options.Value.ConnectionString);
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Author[]>> GetAllAuthorsAsync()
        {
            try
            {
                var entities = await unitOfWork.AuthorRepository.GetAllAsync();
                var authors = mapper.Map<Models.Author[]>(entities);
                return Ok(authors);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthorAsync(int id)
        {
            try
            {
                var entity = await unitOfWork.AuthorRepository.FindAsync(id);
                if (entity != null)
                {
                    var author = mapper.Map<Models.Author>(entity);
                    return Ok(author);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpGet("{id}/Books")]
        public async Task<ActionResult<Book[]>> GetAuthorRelatedBooksAsync(int id)
        {
            try
            {
                var entities = await unitOfWork.BookRepository.GetAuthorRelatedBooksAsync(id);
                if (entities.Any())
                {
                    var books = mapper.Map<Models.Book[]>(entities);
                    return Ok(books);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Author>> AddAuthorASync([FromBody]Author author)
        {
            try
            {
                var entity = mapper.Map<Domain.Entities.Author>(author);
                int entityId = await unitOfWork.AuthorRepository.AddAsync(entity);

                return Created(nameof(GetAuthorAsync), new { id = entityId });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure"); // Fix this, can't be right.
            }
        }

        [HttpPost("{id}/books")]
        public async Task<ActionResult<Book>> AddAuthorRelatedBook(int id, [FromBody] Book book)
        {
            try
            {
                var entity = mapper.Map<Domain.Entities.Book>(book);
                entity.AuthorId = id;
                int entityId = await unitOfWork.BookRepository.AddAsync(entity);

                return Created(nameof(GetAuthorAsync), new { Id = entityId });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthorAsync(int id, [FromBody] Author author)
        {
            try
            {
                var entity = await unitOfWork.AuthorRepository.FindAsync(id);
                if (entity != null)
                {
                    entity.FirstName = author.FirstName;
                    entity.LastName = author.LastName;
                    await unitOfWork.AuthorRepository.UpdateAsync(entity);
                    return Ok();
                }

                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAuthorAsync(int id)
        {
            try
            {
                var entity = await unitOfWork.AuthorRepository.FindAsync(id);
                if (entity != null)
                {
                    var bookEntities = await unitOfWork.BookRepository.GetAuthorRelatedBooksAsync(id);
                    bool deleteSuccessful = false;
                    if(bookEntities != null)
                    {
                        deleteSuccessful = await unitOfWork.BookRepository.RemoveAllAuthorRelatedBooks(id);
                    }
                    if(deleteSuccessful)
                    {
                        await unitOfWork.AuthorRepository.DeleteAsync(id);
                        return Ok();
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
                }

                return BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}