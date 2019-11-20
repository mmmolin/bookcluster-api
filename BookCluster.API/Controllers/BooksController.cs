﻿using System;
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
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BooksController(IOptions<Option> options, IMapper mapper)
        {
            this.unitOfWork = new UnitOfWork(options.Value.ConnectionString);
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Book[]>> GetAllBooks()
        {
            try
            {
                var entities = await unitOfWork.BookRepository.GetAllAsync();
                var books = mapper.Map<Models.Book[]>(entities);
                return Ok(books);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var entity = await unitOfWork.BookRepository.FindAsync(id);
                if(entity != null)
                {
                    var book = mapper.Map<Models.Book>(entity);
                    return Ok(book);
                }

                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }
    }
}