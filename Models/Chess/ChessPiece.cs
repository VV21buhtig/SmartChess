using SmartChess.Models.Chess.Enums;
using System.Collections.Generic;

namespace SmartChess.Models.Chess
{
    public abstract class ChessPiece
    {
        public Color Color { get; }
        public Position Position { get; protected set; }
        public abstract PieceType Type { get; }
        public bool HasMoved { get; protected set; }

        //public string ImagePath { get; protected set; } = string.Empty;
        //public string ImagePath => $"/Resources/Images/{Type}_{Color}.png";
        public string ImagePath
        {
            get
            {
                string colorSuffix = Color == Color.White ? "W" : "B";
                return $"/Resources/Images/{Type}_{colorSuffix}.png";
            }
        }
        protected ChessPiece(Color color, Position position)
        {
            Color = color;
            Position = position;
            HasMoved = false;
        }

        public abstract IEnumerable<Position> GetPossibleMoves(Board board);
        public abstract bool IsValidMove(Position newPosition, Board board);

        public virtual void MoveTo(Position newPosition)
        {
            Position = newPosition;
            HasMoved = true;
            //ImagePath = $"/Resources/Images/{Type}_{Color}.png";
        }

        protected virtual bool IsInBounds(Position pos) => pos.X >= 0 && pos.X < 8 && pos.Y >= 0 && pos.Y < 8;

        public virtual bool IsAttackingSquare(Position target, Board board)
        {
            return GetPossibleMoves(board).Contains(target);
        }
    }
}