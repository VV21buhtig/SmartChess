using SmartChess.Models.Chess.Enums;
using SmartChess.Models.Chess.Pieces;
using SmartChess.Models.Chess;
using System;
using System.Linq;

namespace SmartChess.Models.Chess
{
    public class Board
    {
        private readonly ChessPiece?[,] _squares = new ChessPiece?[8, 8];

        public ChessPiece? this[Position pos]
        {
            get => _squares[pos.X, pos.Y];
            set => _squares[pos.X, pos.Y] = value;
        }

        public ChessPiece? this[int x, int y]
        {
            get => _squares[x, y];
            set => _squares[x, y] = value;
        }

        public void InitializeStandardPosition()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    _squares[x, y] = null;
                }
            }

            for (int x = 0; x < 8; x++)
            {
                _squares[x, 1] = new Pawn(Color.White, new Position(x, 1));
                _squares[x, 6] = new Pawn(Color.Black, new Position(x, 6));
            }

            _squares[0, 0] = new Rook(Color.White, new Position(0, 0));
            _squares[1, 0] = new Knight(Color.White, new Position(1, 0));
            _squares[2, 0] = new Bishop(Color.White, new Position(2, 0));
            _squares[3, 0] = new Queen(Color.White, new Position(3, 0));
            _squares[4, 0] = new King(Color.White, new Position(4, 0));
            _squares[5, 0] = new Bishop(Color.White, new Position(5, 0));
            _squares[6, 0] = new Knight(Color.White, new Position(6, 0));
            _squares[7, 0] = new Rook(Color.White, new Position(7, 0));

            _squares[0, 7] = new Rook(Color.Black, new Position(0, 7));
            _squares[1, 7] = new Knight(Color.Black, new Position(1, 7));
            _squares[2, 7] = new Bishop(Color.Black, new Position(2, 7));
            _squares[3, 7] = new Queen(Color.Black, new Position(3, 7));
            _squares[4, 7] = new King(Color.Black, new Position(4, 7));
            _squares[5, 7] = new Bishop(Color.Black, new Position(5, 7));
            _squares[6, 7] = new Knight(Color.Black, new Position(6, 7));
            _squares[7, 7] = new Rook(Color.Black, new Position(7, 7));
        }

        public bool IsInCheck(Color color)
        {
            Position kingPos = default;
            bool found = false;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = this[x, y];
                    if (piece is King king && king.Color == color)
                    {
                        kingPos = new Position(x, y);
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            if (!found) return false; 
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = this[x, y];
                    if (piece != null && piece.Color != color)
                    {
                        if (piece.IsAttackingSquare(kingPos, this))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsCheckmate(Color color)
        {
            if (!IsInCheck(color)) return false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = this[x, y];
                    if (piece != null && piece.Color == color)
                    {
                        var originalPosition = piece.Position;  // ←←← СОХРАНИТЬ ОРИГИНАЛЬНУЮ ПОЗИЦИЮ
                        var possibleMoves = piece.GetPossibleMoves(this);

                        foreach (var move in possibleMoves)
                        {
                            var capturedPiece = this[move];
                            this[move] = piece;
                            this[originalPosition] = null;  // ←←← Использовать originalPosition
                            piece.MoveTo(move);

                            bool stillInCheck = IsInCheck(color);

                            // ВОССТАНОВЛЕНИЕ
                            this[originalPosition] = piece;
                            piece.MoveTo(originalPosition);  // ←←← ВЕРНУТЬ ОРИГИНАЛЬНУЮ ПОЗИЦИЮ
                            this[move] = capturedPiece;

                            if (!stillInCheck) return false;
                        }
                    }
                }
            }
            return true;
        }
        //public bool IsCheckmate(Color color)
        //{
        //    if (!IsInCheck(color)) return false; 
        //    for (int x = 0; x < 8; x++)
        //    {
        //        for (int y = 0; y < 8; y++)
        //        {
        //            var piece = this[x, y];
        //            if (piece != null && piece.Color == color)
        //            {
        //                var possibleMoves = piece.GetPossibleMoves(this);
        //                foreach (var move in possibleMoves)
        //                {
        //                    var capturedPiece = this[move];
        //                    this[move] = piece;
        //                    this[piece.Position] = null;
        //                    piece.MoveTo(move);

        //                    bool stillInCheck = IsInCheck(color);

        //                    this[piece.Position] = piece;
        //                    piece.MoveTo(new Position(x, y));
        //                    this[move] = capturedPiece;

        //                    if (!stillInCheck)
        //                    {
        //                        return false; 
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}

        public bool IsStalemate(Color color)
        {
            if (IsInCheck(color)) return false;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var piece = this[x, y];
                    if (piece != null && piece.Color == color)
                    {
                        var originalPosition = piece.Position;  // ←←← СОХРАНИТЬ
                        var possibleMoves = piece.GetPossibleMoves(this);

                        foreach (var move in possibleMoves)
                        {
                            var capturedPiece = this[move];
                            this[move] = piece;
                            this[originalPosition] = null;
                            piece.MoveTo(move);

                            bool stillInCheck = IsInCheck(color);

                            this[originalPosition] = piece;
                            piece.MoveTo(originalPosition);  // ←←← ВЕРНУТЬ
                            this[move] = capturedPiece;

                            if (!stillInCheck) return false;
                        }
                    }
                }
            }
            return true;
        }
        //public bool IsStalemate(Color color)
        //{
        //    if (IsInCheck(color)) return false;

        //    for (int x = 0; x < 8; x++)
        //    {
        //        for (int y = 0; y < 8; y++)
        //        {
        //            var piece = this[x, y];
        //            if (piece != null && piece.Color == color)
        //            {
        //                var possibleMoves = piece.GetPossibleMoves(this);
        //                if (possibleMoves.Any())
        //                {
        //                    return false; 
        //                }
        //            }
        //        }
        //    }
        //    return true; 
        //}
    }
}