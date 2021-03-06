﻿using System.Linq;
using Distributed.Models;
using Microsoft.AspNetCore.Mvc;
using Distributed.FormModels;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Distributed.Controllers
{
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private readonly GamesContext _context;

        public GenresController(GamesContext context)
        {
            _context = context;
        }

        // GET api/genres
        [HttpGet]
        public IQueryable<Genre> Get()
        {
            IQueryable<Genre> genres = _context.Genre;
            return genres;
        }

        // GET api/genres/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            Genre genre = _context.Genre.SingleOrDefault(m => m.Id == id);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        // POST api/genres
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public object Post([FromBody]GenrePostDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Genre genre = model.GetGenre();
                    _context.Add(genre);
                    _context.SaveChanges();
                    return StatusCode(201, genre);
                }
                catch (Exception exception)
                {
                    return BadRequest(new { exception = exception.InnerException.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // PUT api/genres/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public object Put(int id, [FromBody]GenrePostDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Genre genre = _context.Genre.SingleOrDefault(m => m.Id == id);
                    if (genre == null)
                    {
                        return NotFound();
                    }
                    model.UpdateGenre(genre);
                    _context.Update(genre);
                    _context.SaveChanges();
                    return StatusCode(200, genre);
                }
                catch (Exception exception)
                {
                    return BadRequest(new { exception = exception.InnerException.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE api/genres/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public object Delete(int id)
        {
            Genre genre = _context.Genre.SingleOrDefault(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }
            try
            {
                _context.Genre.Remove(genre);
                _context.SaveChanges();
                return StatusCode(204);
            }
            catch (Exception exception)
            {
                return BadRequest(new { exception = exception.InnerException.Message });
            }
        }
    }
}
