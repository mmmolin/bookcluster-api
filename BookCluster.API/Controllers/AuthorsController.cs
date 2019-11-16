using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper mapper;

        public AuthorsController(IOptions<Option> options, IMapper mapper)
        {
            this.unitOfWork = new UnitOfWork(options.Value.ConnectionString);
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAllAuthors()
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
        public async Task<ActionResult<Author>> GetAuthorAndBooks(int id)
        {
            try
            {
                var entity = await unitOfWork.AuthorRepository.GetAuthorAndBooksAsync(id);
                if(entity != null)
                {
                    var author = mapper.Map<Models.Author>(entity);
                    
                    if(entity.Books != null)
                    {
                        author.Books = entity.Books.Select(x => new Book { Id = x.Id, Title = x.Title }).ToList();
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