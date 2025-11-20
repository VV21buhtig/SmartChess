using Microsoft.EntityFrameworkCore;
using SmartChess.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Data.Repository
{
    public class MoveRepository : IMoveRepository
    {
        private readonly AppDbContext _context;

        public MoveRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Move> CreateMoveAsync(Move move)
        {
            _context.Moves.Add(move);
            await _context.SaveChangesAsync();
            return move;
        }

        public async Task<List<Move>> GetMovesByGameIdAsync(int gameId)
        {
            return await _context.Moves.Where(m => m.GameId == gameId).ToListAsync();
        }
    }
}