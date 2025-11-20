using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using SmartChess.Models.Entities;
using System.Threading.Tasks;

namespace SmartChess.Services
{
    public class GameSessionService
    {
        public Board CurrentBoard { get; private set; } = new Board();
        public Models.Chess.Enums.Color CurrentPlayer { get; private set; } = Models.Chess.Enums.Color.White;
        public Models.Chess.Enums.GameState GameState { get; private set; } = Models.Chess.Enums.GameState.InProgress;

        private readonly IChessEngine _chessEngine;
        private readonly DatabaseService _databaseService;
        private User? _currentUser;
        private Game? _currentGame;

        public GameSessionService(IChessEngine chessEngine, DatabaseService databaseService)
        {
            _chessEngine = chessEngine;
            _databaseService = databaseService;
        }

        public void InitializeGame()
        {
            _chessEngine.InitializeGame(); // Вызов метода из IChessEngine
            CurrentBoard = _chessEngine.CurrentBoard;
            CurrentPlayer = _chessEngine.CurrentPlayer;
            GameState = _chessEngine.GameState;

            // We can't do async operations in initialization, so we'll create the game when needed
            // Or just initialize the game object without saving to DB here
            if (_currentUser != null)
            {
                _currentGame = new Game { UserId = _currentUser.Id };
                // We'll save to DB when the first move is made or when explicitly requested
            }
        }

        public async Task StartNewGameAsync(User user)
        {
            _currentUser = user;
            InitializeGame(); // Используем внутренний метод
            
            // Создаем новую игру в базе данных
            if (_currentUser != null)
            {
                _currentGame = new Game 
                { 
                    UserId = _currentUser.Id,
                    StartTime = DateTime.Now,
                    Result = "In Progress"
                };
                _currentGame = await _databaseService.CreateGameAsync(_currentGame);
            }
        }

        public async Task StartNewGameAsync()
        {
            // Parameterless version - starts a game without user (for guest play)
            _currentUser = null;
            InitializeGame();
        }

        public async Task<bool> MakeMoveAsync(Position from, Position to)
        {
            System.Diagnostics.Trace.WriteLine($"=== GAME SESSION MAKE MOVE: {from} -> {to} ===");
            if (_currentGame == null)
            {
                System.Diagnostics.Trace.WriteLine("=== ERROR: No current game ===");
                return false; // Игра не начата
            }

            // Вызов метода из ChessEngine
            bool moveSuccess = await _chessEngine.MakeMoveAsync(from, to);
            System.Diagnostics.Trace.WriteLine($"ChessEngine move success: {moveSuccess}");

            if (moveSuccess)
            {
                // Обновление состояния сессии
                CurrentBoard = _chessEngine.CurrentBoard;
                CurrentPlayer = _chessEngine.CurrentPlayer;
                GameState = _chessEngine.GameState;
                System.Diagnostics.Trace.WriteLine("=== MOVE SUCCESS IN SESSION ===");
                // Запись хода в БД
                var move = new Move
                {
                    GameId = _currentGame.Id,
                    MoveNumber = _currentGame.MoveCount + 1,
                    FromPosition = $"{(char)('a' + from.X)}{(char)('1' + from.Y)}",
                    ToPosition = $"{(char)('a' + to.X)}{(char)('1' + to.Y)}",
                    PieceType = CurrentBoard[to]?.Type.ToString() ?? "",
                    Color = CurrentBoard[to]?.Color.ToString() ?? "",
                    IsCapture = CurrentBoard[to] != null,
                    CapturedPiece = CurrentBoard[to]?.Type.ToString()
                };
                await _databaseService.CreateMoveAsync(move);
                _currentGame.MoveCount++;
                
                // Проверяем, завершилась ли игра
                if (GameState == Models.Chess.Enums.GameState.Checkmate)
                {
                    // Мат - победитель - игрок, который НЕ в шахе (после хода)
                    // CurrentPlayer уже изменился, поэтому побеждает предыдущий игрок
                    var winnerColor = CurrentPlayer == Models.Chess.Enums.Color.White ? Models.Chess.Enums.Color.Black : Models.Chess.Enums.Color.White;
                    _currentGame.Result = $"Checkmate - {winnerColor} wins";
                    _currentGame.EndTime = DateTime.Now;
                    await _databaseService.UpdateGameAsync(_currentGame); // Обновляем игру в БД
                }
                else if (GameState == Models.Chess.Enums.GameState.Stalemate)
                {
                    _currentGame.Result = "Stalemate - Draw";
                    _currentGame.EndTime = DateTime.Now;
                    await _databaseService.UpdateGameAsync(_currentGame); // Обновляем игру в БД
                }
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("=== MOVE FAILED IN SESSION ===");
            }

            return moveSuccess;
        }

        public async Task<Models.Chess.Enums.GameState> GetGameStateAsync()
        {
            return await _chessEngine.GetGameStateAsync();
        }
    }
}