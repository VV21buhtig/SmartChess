using SmartChess.Models.Chess.Enums;
using System;
using System.Collections.Generic;

namespace SmartChess.Models.Chess.Pieces
{
    public class Pawn : ChessPiece
    {
        public override PieceType Type => PieceType.Pawn;
        public bool CanBeCapturedEnPassant { get; set; }

        public Pawn(Color color, Position position) : base(color, position)
        {
            //ImagePath = color == Color.White ? "/Resources/Images/Pawn_W.png" : "/Resources/Images/Pawn_B.png";
            CanBeCapturedEnPassant = false;
        }

        public override IEnumerable<Position> GetPossibleMoves(Board board)
        {
            var moves = new List<Position>();
            int direction = Color == Color.White ? 1 : -1;
            int startRow = Color == Color.White ? 1 : 6;
            Position currentPos = Position;

            var oneStep = new Position(currentPos.X, currentPos.Y + direction);
            if (IsInBounds(oneStep) && board[oneStep] == null)
            {
                moves.Add(oneStep);

                if (!HasMoved && currentPos.Y == startRow)
                {
                    var twoSteps = new Position(currentPos.X, currentPos.Y + 2 * direction);
                    if (IsInBounds(twoSteps) && board[twoSteps] == null)
                    {
                        moves.Add(twoSteps);
                    }
                }
            }
            for (int dx = -1; dx <= 1; dx += 2)
            {
                var attackPos = new Position(currentPos.X + dx, currentPos.Y + direction);
                if (IsInBounds(attackPos))
                {
                    var targetPiece = board[attackPos];
                    if (targetPiece != null && targetPiece.Color != Color)
                    {
                        moves.Add(attackPos);
                    }
                }
            }
            moves.AddRange(GetEnPassantMoves(board));

            return moves;
        }

        private IEnumerable<Position> GetEnPassantMoves(Board board)
        {
            var moves = new List<Position>();
            int direction = Color == Color.White ? 1 : -1;
            int enemyPawnRow = Color == Color.White ? 4 : 3;

            if (Position.Y == enemyPawnRow)
            {
                for (int dx = -1; dx <= 1; dx += 2)
                {
                    var sidePos = new Position(Position.X + dx, Position.Y);
                    if (IsInBounds(sidePos))
                    {
                        var sidePiece = board[sidePos];
                        if (sidePiece is Pawn enemyPawn && enemyPawn.Color != Color && enemyPawn.CanBeCapturedEnPassant)
                        {
                            moves.Add(new Position(Position.X + dx, Position.Y + direction));
                        }
                    }
                }
            }
            return moves;
        }

        public bool CanPromote()
        {
            int promotionRow = Color == Color.White ? 7 : 0;
            return Position.Y == promotionRow;
        }

        public override bool IsValidMove(Position newPosition, Board board)
        {
            return GetPossibleMoves(board).Contains(newPosition);
        }

        public override bool IsAttackingSquare(Position target, Board board)
        {
            int direction = Color == Color.White ? 1 : -1;
            var leftAttack = new Position(Position.X - 1, Position.Y + direction);
            var rightAttack = new Position(Position.X + 1, Position.Y + direction);
            return (leftAttack == target || rightAttack == target) && IsInBounds(target);
        }

        public override void MoveTo(Position newPosition)
        {
            var oldPosition = Position;
            base.MoveTo(newPosition);

            int startRow = Color == Color.White ? 1 : 6;
            if (Math.Abs(newPosition.Y - oldPosition.Y) == 2)
            {
                CanBeCapturedEnPassant = true;
            }
            else
            {
                CanBeCapturedEnPassant = false;
            }
        }
    }
}