using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using System.Threading.Tasks;

namespace SmartChess.Algorithms
{
    public class GameAnalyzer
    {
        public async Task<int> AnalyzeDepthAsync(Board board, int depth)
        {
            if (depth <= 0)
            {
                return 1; 
            }

            int totalPositions = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = board[x, y];
                    if (piece != null)
                    {
                        var moves = piece.GetPossibleMoves(board);
                        foreach (var move in moves)
                        {
                            var capturedPiece = board[move];
                            board[move] = piece;
                            board[piece.Position] = null;
                            piece.MoveTo(move);

                            totalPositions += await AnalyzeDepthAsync(board, depth - 1);

                            board[piece.Position] = piece;
                            piece.MoveTo(new Position(x, y));
                            board[move] = capturedPiece;
                        }
                    }
                }
            }
            return totalPositions;
        }
    }
}