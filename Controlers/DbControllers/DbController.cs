using Microsoft.EntityFrameworkCore;

namespace Bot_start.Controlers
{
    public class DbController
    {
        public static AppDbContext DB = null;
        public DbController()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            DB = new AppDbContext(configuration);
        }
    }
}
