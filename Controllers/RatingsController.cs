﻿using System.Linq;
using Distributed.Models;
using Microsoft.AspNetCore.Mvc;
using Distributed.FormModels;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Distributed.Controllers
{
    [Route("api/[controller]")]
    public class RatingsController : Controller
    {
        private readonly GamesContext _context;

        public RatingsController(GamesContext context)
        {
            _context = context;
        }

        // GET api/ratings
        [HttpGet]
        public IQueryable<Rating> Get()
        {
            IQueryable<Rating> ratings = _context.Rating;
            return ratings;
        }

        // GET api/ratings/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            Rating rating = _context.Rating.SingleOrDefault(m => m.Id == id);

            if (rating == null)
            {
                return NotFound();
            }

            return rating;
        }

        // POST api/ratings
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public object Post([FromBody]RatingPostDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Rating rating = model.GetRating();
                    _context.Add(rating);
                    _context.SaveChanges();
                    return StatusCode(201, rating);
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

        // PUT api/ratings/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public object Put(int id, [FromBody]RatingPostDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Rating rating = _context.Rating.SingleOrDefault(m => m.Id == id);
                    if (rating == null)
                    {
                        return NotFound();
                    }
                    model.UpdateRating(rating);
                    _context.Update(rating);
                    _context.SaveChanges();
                    return StatusCode(200, rating);
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

        // DELETE api/ratings/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public object Delete(int id)
        {
            Rating rating = _context.Rating.SingleOrDefault(m => m.Id == id);
            if (rating == null)
            {
                return NotFound();
            }
            try
            {
                _context.Rating.Remove(rating);
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
