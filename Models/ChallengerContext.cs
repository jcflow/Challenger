using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("challenger"));
        }
    }
}
