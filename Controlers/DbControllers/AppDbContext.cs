using Bot_start.Models;
using Microsoft.EntityFrameworkCore;

namespace Bot_start.Controlers
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = LoginParameters.GetConnectioString();
            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<SentItem> SentItems { get; set; }
    }
}
