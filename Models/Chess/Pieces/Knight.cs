using SmartChess.Models.Chess.Enums;
using System.Collections.Generic;

namespace SmartChess.Models.Chess.Pieces
{
    public class Knight : ChessPiece
    {
        public override PieceType Type => PieceType.Knight;

        public Knight(Color color, Position position) : base(color, position)
        {
            //ImagePath = color == Color.White ? "/Resources/Images/Knight_W.png" : "/Resources/Images/Knight_B.png";
        }

        public override IEnumerable<Position> GetPossibleMoves(Board board)
        {
            var moves = new List<Position>();
            int[] dx = { 2, 1, -1, -2, -2, -1, 1, 2 };
            int[] dy = { 1, 2, 2, 1, -1, -2, -2, -1 };

            for (int i = 0; i < 8; i++)
            {
                var newPos = new Position(Position.X + dx[i], Position.Y + dy[i]);
                if (IsInBounds(newPos) &&
                    (board[newPos] == null || board[newPos].Color != Color))
                {
                    moves.Add(newPos);
                }
            }
            return moves;
        }

        public override bool IsValidMove(Position newPosition, Board board)
        {
            return GetPossibleMoves(board).Contains(newPosition);
        }

        public override bool IsAttackingSquare(Position target, Board board)
        {
            return GetPossibleMoves(board).Contains(target);
        }
    }
}