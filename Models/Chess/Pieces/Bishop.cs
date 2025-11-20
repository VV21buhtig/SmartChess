using SmartChess.Models.Chess.Enums;
using System.Collections.Generic;

namespace SmartChess.Models.Chess.Pieces
{
    public class Bishop : ChessPiece
    {
        public override PieceType Type => PieceType.Bishop;

        public Bishop(Color color, Position position) : base(color, position)
        {
            //ImagePath = color == Color.White ? "/Resources/Images/Bishop_W.png" : "/Resources/Images/Bishop_B.png";
        }

        public override IEnumerable<Position> GetPossibleMoves(Board board)
        {
            var moves = new List<Position>();
            int[] dx = { -1, -1, 1, 1 };
            int[] dy = { -1, 1, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int x = Position.X + dx[i];
                int y = Position.Y + dy[i];
                while (IsInBounds(new Position(x, y)))
                {
                    var currentPos = new Position(x, y);
                    var pieceAtPos = board[currentPos];
                    if (pieceAtPos == null)
                    {
                        moves.Add(currentPos);
                    }
                    else
                    {
                        if (pieceAtPos.Color != Color)
                        {
                            moves.Add(currentPos);
                        }
                        break;
                    }
                    x += dx[i];
                    y += dy[i];
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