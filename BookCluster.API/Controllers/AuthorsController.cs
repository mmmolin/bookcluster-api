using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookCluster.API.Models;
using BookCluster.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookCluster.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        public AuthorsController(IOptions<Option> options)
        {
            this.unitOfWork = new UnitOfWork(options.Value.ConnectionString);            
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAllAuthors()
        {
            try
            {
                var results = await unitOfWork.AuthorRepository.GetAllAsync();
                var authors = new List<Author>();
                foreach (var entity in results)
                {
                    var author = new Author
                    {
                        Id = entity.Id,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName
                    };
                    authors.Add(author);
                }
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
                var result = await unitOfWork.AuthorRepository.FindAsync(id);
                var author = new Author();
                if (result != null)
                {
                    author = new Author
                    {
                        Id = result.Id,
                        FirstName = result.FirstName,
                        LastName = result.LastName
                    };
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
        public async Task<ActionResult<Author>> GetAuthorAndBooks(int id)
        {
            try
            {
                var result = await unitOfWork.AuthorRepository.GetAuthorAndBooksAsync(id);
                if(result != null)
                {
                    var author = new Author
                    {
                        Id = result.Id,
                        FirstName = result.FirstName,
                        LastName = result.LastName
                    };
                    
                    if(result.Books != null)
                    {
                        author.Books = result.Books.Select(x => new Book { Id = x.Id, Title = x.Title }).ToList();
                    }
                    return Ok(author);
                }
                return NotFound();
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }            
        }
    }
}