using SmartChess.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Data.Repository
{
    public interface IGameRepository
    {
        Task<Game> CreateGameAsync(Game game);
        Task<Game?> GetGameByIdAsync(int id);
        Task<List<Game>> GetGamesByUserIdAsync(int userId);
    }
}