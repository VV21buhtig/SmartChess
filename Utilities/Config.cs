using Microsoft.Extensions.Configuration;

namespace SmartChess.Utilities
{
    public static class Config
    {
        private static IConfiguration? _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConnectionString()
        {
            return _configuration?.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=SmartChessDB;Trusted_Connection=true;MultipleActiveResultSets=true";
        }
    }
}