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

        public async Task InitializeGame()
        {
            _chessEngine.InitializeGame(); // Вызов метода из IChessEngine
            CurrentBoard = _chessEngine.CurrentBoard;
            CurrentPlayer = _chessEngine.CurrentPlayer;
            GameState = _chessEngine.GameState;

            if (_currentUser != null)
            {
                _currentGame = new Game { UserId = _currentUser.Id };
                _currentGame = await _databaseService.CreateGameAsync(_currentGame);
            }
        }

        public async Task StartNewGameAsync(User user)
        {
            _currentUser = user;
            InitializeGame(); // Используем внутренний метод
            // ... (логика создания игры в БД)
        }

        public async Task<bool> MakeMoveAsync(Position from, Position to)
        {
            //System.Diagnostics.Trace.WriteLine($"=== GAME SESSION MAKE MOVE: {from} -> {to} ===");
            //if (_currentGame == null)
            //{
            //    System.Diagnostics.Trace.WriteLine("=== ERROR: No current game ===");
            //    return false; // Игра не начата
            //}

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
                // ... (логика создания и сохранения Move в БД)
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