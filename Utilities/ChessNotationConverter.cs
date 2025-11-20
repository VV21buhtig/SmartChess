using SmartChess.Models.Chess;
using System;

namespace SmartChess.Utilities
{
    public static class ChessNotationConverter
    {
        public static Position FromChessNotation(string notation)
        {
            if (string.IsNullOrEmpty(notation) || notation.Length != 2)
                throw new ArgumentException("Invalid chess notation string.", nameof(notation));

            char file = notation[0];
            char rank = notation[1];

            if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
                throw new ArgumentException("Invalid chess notation characters.", nameof(notation));

            int x = file - 'a';
            int y = rank - '1';
            return new Position(x, y);
        }

        public static string ToChessNotation(Position position)
        {
            if (position.X < 0 || position.X > 7 || position.Y < 0 || position.Y > 7)
                throw new InvalidOperationException("Position is out of board bounds (0-7).");
            return $"{(char)('a' + position.X)}{(char)('1' + position.Y)}";
        }
    }
}