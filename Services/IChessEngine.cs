using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Services
{
    public interface IChessEngine
    {
        Board CurrentBoard { get; }
        Color CurrentPlayer { get; }
        GameState GameState { get; }

        void InitializeGame();
        Task<bool> MakeMoveAsync(Position from, Position to);
        Task<List<Position>> GetPossibleMovesAsync(Position position);
        Task<GameState> GetGameStateAsync();
        Task<Board> GetCurrentBoardStateAsync();
    }
}