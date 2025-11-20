using Microsoft.EntityFrameworkCore;
using SmartChess.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Data.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;

        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Game> CreateGameAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<List<Game>> GetGamesByUserIdAsync(int userId)
        {
            return await _context.Games.Where(g => g.UserId == userId).ToListAsync();
        }
    }
}