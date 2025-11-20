using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;

namespace SmartChess.Utilities
{
    public static class Extensions
    {
        public static Color ColorAt(this Board board, Position pos)
        {
            var piece = board[pos];
            return piece?.Color ?? default;
        }

        public static bool IsInCheck(this Board board, Color color)
        {
            return board.IsInCheck(color);
        }

        public static bool IsCheckmate(this Board board, Color color)
        {
            return board.IsCheckmate(color);
        }

        public static bool IsStalemate(this Board board, Color color)
        {
            return board.IsStalemate(color);
        }
    }
}