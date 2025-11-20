using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using SmartChess.Models.Chess.Pieces;
using SmartChess.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Algorithms
{
    public class MoveCalculator
    {
        public async Task<List<Position>> CalculateMovesAsync(Position start, PieceType pieceType, Board board)
        {
            var testPiece = CreateTestPiece(pieceType, start, board.ColorAt(start)); 
            if (testPiece == null) return new List<Position>();

            return new List<Position>(testPiece.GetPossibleMoves(board));
        }

        private ChessPiece? CreateTestPiece(PieceType type, Position pos, Color color)
        {
            return type switch
            {
                PieceType.Pawn => new Pawn(color, pos),
                PieceType.Knight => new Knight(color, pos),
                PieceType.Bishop => new Bishop(color, pos),
                PieceType.Rook => new Rook(color, pos),
                PieceType.Queen => new Queen(color, pos),
                PieceType.King => new King(color, pos),
                _ => null,
            };
        }
    }
}