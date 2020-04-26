using System;
using Microsoft.EntityFrameworkCore;


namespace Models
{
    public class ChallengerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TournamentCategory> TournamentCategories { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Bracket> Brackets { get; set; }

        public ChallengerContext() : base()
        {
        }

        public ChallengerContext(DbContextOptions<ChallengerContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server = 172.18.0.2; Database = challenger; User Id = sa; Password = Password123;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
