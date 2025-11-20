using SmartChess.Algorithms;
using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Services
{
    public class AlgorithmService
    {
        private readonly PathFinder _pathFinder;
        private readonly GameAnalyzer _gameAnalyzer;
        private readonly MoveCalculator _moveCalculator;

        public AlgorithmService(PathFinder pathFinder, GameAnalyzer gameAnalyzer, MoveCalculator moveCalculator)
        {
            _pathFinder = pathFinder;
            _gameAnalyzer = gameAnalyzer;
            _moveCalculator = moveCalculator;
        }

        public async Task<(List<Position> path, int cost)> FindPathAsync(Position start, Position target, Board board)
        {
            return await _pathFinder.FindPathAsync(start, target, board);
        }

        public async Task<int> AnalyzeGameDepthAsync(Board board, int depth)
        {
            return await _gameAnalyzer.AnalyzeDepthAsync(board, depth);
        }

        public async Task<List<Position>> CalculateMovesAsync(Position start, PieceType pieceType, Board board)
        {
            return await _moveCalculator.CalculateMovesAsync(start, pieceType, board);
        }
    }
}