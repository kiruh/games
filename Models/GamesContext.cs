using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Distributed.Models
{
    public class GamesContext : IdentityDbContext
    {
        public GamesContext()
            : base()
        { }

        public GamesContext(DbContextOptions<GamesContext> options)
            : base(options)
        { }

        public DbSet<Distributed.Models.User> User { get; set; }
        public DbSet<Distributed.Models.Game> Game { get; set; }
        public DbSet<Distributed.Models.Genre> Genre { get; set; }
        public DbSet<Distributed.Models.Rating> Rating { get; set; }
    }
}