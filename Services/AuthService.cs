using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using SmartChess.Models.Entities;
using System.Threading.Tasks;

namespace SmartChess.Services
{
    public class AuthService
    {
        private readonly DatabaseService _databaseService;
        private readonly IConfiguration _configuration;

        public AuthService(DatabaseService databaseService, IConfiguration configuration)
        {
            _databaseService = databaseService;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUserAsync(string login, string password)
        {
            var existingUser = await _databaseService.GetUserByLoginAsync(login);
            System.Diagnostics.Trace.WriteLine($"Existing user: {existingUser != null}");
            if (existingUser != null)
            {
                System.Diagnostics.Trace.WriteLine($"=== REGISTER ATTEMPT: {login} ===");
                return false;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User
            {
                Login = login,
                PasswordHash = hashedPassword
            };

            await _databaseService.CreateUserAsync(newUser);
            System.Diagnostics.Trace.WriteLine("USER CREATED SUCCESSFULLY - RETURNING TRUE");
            return true;
        }

        public async Task<User?> AuthenticateUserAsync(string login, string password)
        {
            var user = await _databaseService.GetUserByLoginAsync(login);
            System.Diagnostics.Trace.WriteLine($"=== LOGIN ATTEMPT: {login} ===");
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                user.LastLogin = System.DateTime.Now;
                System.Diagnostics.Trace.WriteLine("PASSWORD CORRECT - RETURNING USER");
                return user;
            }
            System.Diagnostics.Trace.WriteLine("AUTH FAILED - RETURNING NULL");
            return null;
        }
    }
}