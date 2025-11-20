using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Services
{
    public class ChessEngine : IChessEngine
    {
        public Board CurrentBoard { get; private set; } = new Board();
        public Color CurrentPlayer { get; private set; } = Color.White;
        public GameState GameState { get; private set; } = GameState.InProgress;

        public void InitializeGame()
        {
            CurrentBoard.InitializeStandardPosition();
            CurrentPlayer = Color.White;
            GameState = GameState.InProgress;
        }

        public async Task<bool> MakeMoveAsync(Position from, Position to)
        {
            System.Diagnostics.Trace.WriteLine($"=== CHESS ENGINE MOVE: {from} -> {to} ===");

            var piece = CurrentBoard[from];
            if (piece == null)
            {
                System.Diagnostics.Trace.WriteLine("=== ERROR: No piece at from position ===");
                return false;
            }

            if (piece.Color != CurrentPlayer)
            {
                System.Diagnostics.Trace.WriteLine($"=== ERROR: Wrong color. Piece: {piece.Color}, Current: {CurrentPlayer} ===");
                return false;
            }

            if (!piece.IsValidMove(to, CurrentBoard))
            {
                System.Diagnostics.Trace.WriteLine("=== ERROR: Invalid move ===");
                return false;
            }

            System.Diagnostics.Trace.WriteLine("=== MOVE VALID - EXECUTING ===");

            var capturedPiece = CurrentBoard[to];
            CurrentBoard[to] = piece;
            CurrentBoard[from] = null;
            piece.MoveTo(to);

            bool inCheck = CurrentBoard.IsInCheck(CurrentPlayer);
            if (inCheck)
            {
                System.Diagnostics.Trace.WriteLine("=== ERROR: Move puts king in check ===");
                CurrentBoard[from] = piece;
                CurrentBoard[to] = capturedPiece;
                piece.MoveTo(from);
                return false;
            }

            CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;
            System.Diagnostics.Trace.WriteLine($"=== MOVE SUCCESS! New player: {CurrentPlayer} ===");

            // Обновление состояния игры
            if (CurrentBoard.IsCheckmate(CurrentPlayer))
            {
                GameState = GameState.Checkmate;
            }
            else if (CurrentBoard.IsStalemate(CurrentPlayer))
            {
                GameState = GameState.Stalemate;
            }
            else
            {
                GameState = GameState.InProgress;
            }

            return true;
        }
        //public async Task<bool> MakeMoveAsync(Position from, Position to)
        //{
        //    System.Diagnostics.Trace.WriteLine($"=== CHESS ENGINE MOVE: {from} -> {to} ===");
        //    var piece = CurrentBoard[from];
        //    if (piece == null || piece.Color != CurrentPlayer || !piece.IsValidMove(to, CurrentBoard))
        //    {
        //        System.Diagnostics.Trace.WriteLine("=== ERROR: No piece at from position ===");
        //        return false;
        //    }

        //    var capturedPiece = CurrentBoard[to];
        //    CurrentBoard[to] = piece;
        //    CurrentBoard[from] = null;
        //    piece.MoveTo(to);

        //    bool inCheck = CurrentBoard.IsInCheck(CurrentPlayer);
        //    if (inCheck)
        //    {
        //        CurrentBoard[from] = piece;
        //        CurrentBoard[to] = capturedPiece;
        //        piece.MoveTo(from);
        //        return false;
        //    }

        //    CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;

        //    if (CurrentBoard.IsCheckmate(CurrentPlayer))
        //    {
        //        GameState = GameState.Checkmate;
        //    }
        //    else if (CurrentBoard.IsStalemate(CurrentPlayer))
        //    {
        //        GameState = GameState.Stalemate;
        //    }
        //    else
        //    {
        //        GameState = GameState.InProgress;
        //    }

        //    return true;
        //}

        public async Task<List<Position>> GetPossibleMovesAsync(Position position)
        {
            var piece = CurrentBoard[position];
            if (piece == null || piece.Color != CurrentPlayer)
            {
                return new List<Position>();
            }
            var possibleMoves = piece.GetPossibleMoves(CurrentBoard);
            var filteredMoves = new List<Position>();

            foreach (var move in possibleMoves)
            {
                var capturedPiece = CurrentBoard[move];
                CurrentBoard[move] = piece;
                CurrentBoard[piece.Position] = null;
                piece.MoveTo(move);

                if (!CurrentBoard.IsInCheck(CurrentPlayer))
                {
                    filteredMoves.Add(move);
                }

                CurrentBoard[piece.Position] = piece;
                piece.MoveTo(position);
                CurrentBoard[move] = capturedPiece;
            }

            return filteredMoves;
        }

        public async Task<GameState> GetGameStateAsync()
        {
            return GameState;
        }

        public async Task<Board> GetCurrentBoardStateAsync()
        {
            return CurrentBoard;
        }
    }
}