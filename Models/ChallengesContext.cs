using System;
using Microsoft.EntityFrameworkCore;


namespace Models
{
    public class ChallengesContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Bracket> Brackets { get; set; }

        public ChallengesContext() : base()
        {
        }

        public ChallengesContext(DbContextOptions<ChallengesContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server = 127.0.0.1; Database = challenger; User Id = sa; Password = Password123;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
