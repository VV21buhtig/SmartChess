using SmartChess.Models.Entities;
using System.Threading.Tasks;

namespace SmartChess.Data.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByLoginAsync(string login);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
    }
}