using SmartChess.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Data.Repository
{
    public interface IMoveRepository
    {
        Task<Move> CreateMoveAsync(Move move);
        Task<List<Move>> GetMovesByGameIdAsync(int gameId);
    }
}