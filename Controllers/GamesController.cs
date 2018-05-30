﻿using System.Linq;
using Distributed.Models;
using Microsoft.AspNetCore.Mvc;
using Distributed.FormModels;
using System;
using Microsoft.EntityFrameworkCore;

namespace Distributed.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly GamesContext _context;

        public GamesController(GamesContext context)
        {
            _context = context;
        }

        // GET api/games
        [HttpGet]
        public IQueryable<Game> Get()
        {
            IQueryable<Game> games = _context.Game
                .Include(m => m.Genre)
                .Include(m => m.Rating);
            return games;
        }

        // GET api/games/5
        [HttpGet("{id}")]
        public object Get(int id)
        {
            Game game = _context.Game
                .Include(m => m.Genre)
                .Include(m => m.Rating)
                .SingleOrDefault(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }
            return game;
        }

        // POST api/games
        [HttpPost]
        public object Post([FromBody]GamePostDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Game game = Game.CreateFromPostDto(model);
                    _context.Add(game);
                    _context.SaveChanges();
                    return StatusCode(201, game);
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

        // PUT api/games/5
        [HttpPut("{id}")]
        public object Put(int id, [FromBody]GamePostDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Game game = _context.Game.SingleOrDefault(m => m.Id == id);
                    if (game == null)
                    {
                        return NotFound();
                    }
                    game.UpdateFromPostDto(model);
                    _context.Update(game);
                    _context.SaveChanges();
                    return StatusCode(200, game);
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

        // DELETE api/games/5
        [HttpDelete("{id}")]
        public object Delete(int id)
        {
            Game game = _context.Game.SingleOrDefault(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }
            try
            {
                _context.Game.Remove(game);
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
