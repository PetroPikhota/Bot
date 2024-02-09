
namespace Bot_start.Controlers
{
    public class DbController
    {
        private static AppDbContext _DB = null;
        public DbController()
        {
            if (_DB == null)
            {
                IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
                _DB = new AppDbContext(configuration);
            }
        }

        public AppDbContext GetDb()
        {
            return _DB;
        }

    }
}
