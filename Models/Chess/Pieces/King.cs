using SmartChess.Models.Chess.Enums;
using System;
using System.Collections.Generic;

namespace SmartChess.Models.Chess.Pieces
{
    public class King : ChessPiece
    {
        public override PieceType Type => PieceType.King;

        public King(Color color, Position position) : base(color, position)
        {
            //ImagePath = color == Color.White ? "/Resources/Images/King_W.png" : "/Resources/Images/King_B.png";
        }

        public override IEnumerable<Position> GetPossibleMoves(Board board)
        {
            var moves = new List<Position>();

            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                var newPos = new Position(Position.X + dx[i], Position.Y + dy[i]);
                if (IsInBounds(newPos) &&
                    (board[newPos] == null || board[newPos].Color != Color))
                {
                    moves.Add(newPos);
                }
            }

            moves.AddRange(GetCastlingMoves(board));

            return moves;
        }

        private IEnumerable<Position> GetCastlingMoves(Board board)
        {
            var moves = new List<Position>();

            if (HasMoved || board.IsInCheck(Color))
                return moves;

            int kingRow = Color == Color.White ? 0 : 7;

            if (CanCastleKingside(board, kingRow))
            {
                moves.Add(new Position(6, kingRow));
            }

            if (CanCastleQueenside(board, kingRow))
            {
                moves.Add(new Position(2, kingRow));
            }

            return moves;
        }

        private bool CanCastleKingside(Board board, int kingRow)
        {
            var kingsideRook = board[7, kingRow] as Rook;
            if (kingsideRook == null || kingsideRook.Color != Color || kingsideRook.HasMoved)
                return false;

            for (int x = 5; x <= 6; x++)
            {
                if (board[x, kingRow] != null)
                    return false;
            }

            return !board.IsInCheck(Color) &&
                   !WouldBeInCheckAfterMove(new Position(5, kingRow), board) &&
                   !WouldBeInCheckAfterMove(new Position(6, kingRow), board);
        }

        private bool CanCastleQueenside(Board board, int kingRow)
        {
            var queensideRook = board[0, kingRow] as Rook;
            if (queensideRook == null || queensideRook.Color != Color || queensideRook.HasMoved)
                return false;

            for (int x = 1; x <= 3; x++)
            {
                if (board[x, kingRow] != null)
                    return false;
            }

            return !board.IsInCheck(Color) &&
                   !WouldBeInCheckAfterMove(new Position(3, kingRow), board) &&
                   !WouldBeInCheckAfterMove(new Position(2, kingRow), board);
        }

        private bool WouldBeInCheckAfterMove(Position newPosition, Board board)
        {
            var originalPosition = Position;
            var originalPiece = board[newPosition];

            board[newPosition] = this;
            board[originalPosition] = null;
            var tempPosition = Position;
            Position = newPosition;

            bool inCheck = board.IsInCheck(Color);

            board[originalPosition] = this;
            board[newPosition] = originalPiece;
            Position = tempPosition;

            return inCheck;
        }

        public override bool IsValidMove(Position newPosition, Board board)
        {
            return GetPossibleMoves(board).Contains(newPosition);
        }

        public override bool IsAttackingSquare(Position target, Board board)
        {
            int dx = Math.Abs(target.X - Position.X);
            int dy = Math.Abs(target.Y - Position.Y);
            return dx <= 1 && dy <= 1 && (dx != 0 || dy != 0);
        }
    }
}