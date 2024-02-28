namespace FarmProject.ConnectionString
{
    public class Connection
    {
        private readonly IConfiguration _configuration;
        public readonly string Cnstr;

        public Connection(IConfiguration configuration)
        {
            _configuration = configuration;
            Cnstr = configuration.GetConnectionString("DefaultConnection");
        }
    }
}