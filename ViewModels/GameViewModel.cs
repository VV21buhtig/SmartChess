using SmartChess.Commands;
using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using SmartChess.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartChess.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly GameSessionService _gameSessionService; // Используем GameSessionService
        private Board _currentBoard = new Board();
        private Models.Chess.Enums.Color _currentPlayer = Models.Chess.Enums.Color.White;
        private Models.Chess.Enums.GameState _gameState = Models.Chess.Enums.GameState.InProgress;
        private string _statusMessage = "Игра началась! Ход белых.";
        private Position? _selectedPosition;
        public ObservableCollection<ChessSquareViewModel> BoardSquares { get; } = new ObservableCollection<ChessSquareViewModel>();
        public RelayCommand<Position> SelectPieceCommand { get; }
        public RelayCommand<Position> MovePieceCommand { get; }
        public RelayCommand NavigateToHistoryCommand { get; }
        public RelayCommand NavigateToProfileCommand { get; }
        // Изменён конструктор: принимает GameSessionService через DI
        public GameViewModel(GameSessionService gameSessionService)
        {
            _gameSessionService = gameSessionService;
            SelectPieceCommand = new RelayCommand<Position>(SelectPiece);
            MovePieceCommand = new RelayCommand<Position>(async (position) => await MovePiece(position));
            NavigateToHistoryCommand = new RelayCommand(() => _mainViewModel.NavigateToHistoryCommand.Execute(null));
            NavigateToProfileCommand = new RelayCommand(() => _mainViewModel.NavigateToProfileCommand.Execute(null));
            _gameSessionService.InitializeGame();
            _currentBoard = _gameSessionService.CurrentBoard;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            System.Diagnostics.Trace.WriteLine($"=== INIT BOARD - Squares count: {BoardSquares.Count} ===");

            BoardSquares.Clear();

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var position = new Position(x, y);
                    var piece = _currentBoard[position];

                    BoardSquares.Add(new ChessSquareViewModel
                    {
                        Position = position,
                        Piece = piece,
                        BackgroundColor = GetOriginalColor(position)
                    });
                }
            }

            OnPropertyChanged(nameof(BoardSquares));
            System.Diagnostics.Trace.WriteLine($"=== BOARD UPDATED - New squares: {BoardSquares.Count} ===");
        }
        //private void InitializeBoard()
        //{
        //    BoardSquares.Clear();
        //    for (int y = 0; y < 8; y++)
        //    {
        //        for (int x = 0; x < 8; x++)
        //        {
        //            var position = new Position(x, y);
        //            var piece = _currentBoard[position];

        //            BoardSquares.Add(new ChessSquareViewModel
        //            {
        //                Position = position,
        //                Piece = piece,
        //                BackgroundColor = (x + y) % 2 == 0 ? Brushes.White : Brushes.LightGray
        //            });
        //        }
        //    }
        //    OnPropertyChanged(nameof(BoardSquares));
        //}
        public Board CurrentBoard
        {
            get => _currentBoard;
            set
            {
                _currentBoard = value;
                OnPropertyChanged(nameof(CurrentBoard));
            }
        }

        public Models.Chess.Enums.Color CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        public Models.Chess.Enums.GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                OnPropertyChanged(nameof(GameState));
                UpdateStatusMessage();
            }
        }

        private void SelectPiece(Position position)
        {
            System.Diagnostics.Trace.WriteLine($"=== SELECT PIECE: {position} ===");
            // Если уже есть выделенная фигура - пытаемся сделать ход
            if (_selectedPosition != null)
            {
                var piece = _currentBoard[_selectedPosition.Value];
                var possibleMoves = piece.GetPossibleMoves(_currentBoard);

                System.Diagnostics.Trace.WriteLine($"Selected position: {_selectedPosition}, Target: {position}");
                System.Diagnostics.Trace.WriteLine($"Possible moves count: {possibleMoves.Count()}");

                // Если кликнули на возможный ход - двигаем
                if (possibleMoves.Contains(position))
                {
                    System.Diagnostics.Trace.WriteLine("=== VALID MOVE - MAKING MOVE ===");
                    MakeMoveCommand(_selectedPosition.Value, position);
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("=== INVALID MOVE ===");
                }
                // В любом случае снимаем выделение
                ClearSelection();
                return;
            }

            // Если нет выделения - выделяем фигуру
            var clickedPiece = _currentBoard[position];
            if (clickedPiece != null && clickedPiece.Color == _currentPlayer)
            {
                System.Diagnostics.Trace.WriteLine($"=== SELECTING PIECE: {clickedPiece.Type} at {position} ===");
                _selectedPosition = position;
                HighlightPossibleMoves(position);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("=== NO PIECE OR WRONG COLOR ===");
            }
        }
        private void ClearSelection()
        {
            _selectedPosition = null;
            
            foreach (var square in BoardSquares)
            {
                square.BackgroundColor = GetOriginalColor(square.Position);
            }
        }

        private void HighlightPossibleMoves(Position position)
        {
            var piece = _currentBoard[position];
            var possibleMoves = piece.GetPossibleMoves(_currentBoard);

            foreach (var square in BoardSquares)
            {
                // Возвращаем оригинальный цвет
                square.BackgroundColor = GetOriginalColor(square.Position);

                // Подсвечиваем возможные ходы зеленым
                if (possibleMoves.Contains(square.Position))
                {
                    square.BackgroundColor = Brushes.LightGreen;
                }
            }

            // Подсвечиваем выбранную фигуру желтым
            var selectedSquare = BoardSquares.FirstOrDefault(s => s.Position == position);
            if (selectedSquare != null)
            {
                selectedSquare.BackgroundColor = Brushes.Yellow;
            }
        }

        private Brush GetOriginalColor(Position position)
        {
            return (position.X + position.Y) % 2 == 0 ? Brushes.White : Brushes.LightGray;
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }
        private async Task MovePiece(Position targetPosition)
        {
            if (_selectedPosition != null)
            {
                await MakeMoveCommand(_selectedPosition.Value, targetPosition);
                ClearSelection();
            }
        }
        public async void StartNewGameCommand()
        {
            // Предположим, _mainViewModel доступен через DI или передаётся в конструктор
            // await _gameSessionService.StartNewGameAsync(_mainViewModel.CurrentUser);
            // Или если пользователь передаётся напрямую
            // await _gameSessionService.StartNewGameAsync(someUser);

            // Для демонстрации, просто инициализируем игру через GameSessionService
            // _gameSessionService.InitializeGame(); // Вызов метода из GameSessionService
            // CurrentBoard = _gameSessionService.CurrentBoard;
            // CurrentPlayer = _gameSessionService.CurrentPlayer;
            // GameState = _gameSessionService.GameState;
             StatusMessage = "Игра началась! Ход белых.";
        }

        public async Task MakeMoveCommand(Position from, Position to)
        {
            System.Diagnostics.Trace.WriteLine($"=== MAKE MOVE: {from} -> {to} ===");
            var success = await _gameSessionService.MakeMoveAsync(from, to);
            System.Diagnostics.Trace.WriteLine($"Move success: {success}");
            if (success)
            {
                // Обновление состояния из GameSessionService
                CurrentBoard = _gameSessionService.CurrentBoard;
                CurrentPlayer = _gameSessionService.CurrentPlayer;
                GameState = _gameSessionService.GameState;
                System.Diagnostics.Trace.WriteLine("=== UPDATING BOARD ===");
                InitializeBoard();
                //CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("=== MOVE FAILED ===");
            }
        }

        private void UpdateStatusMessage()
        {
            switch (GameState)
            {
                case Models.Chess.Enums.GameState.Check:
                    StatusMessage = $"Шах! Ход {_currentPlayer} игрока.";
                    break;
                case Models.Chess.Enums.GameState.Checkmate:
                    StatusMessage = $"Мат! Победил {(_currentPlayer == Models.Chess.Enums.Color.White ? "черный" : "белый")} игрок.";
                    break;
                case Models.Chess.Enums.GameState.Stalemate:
                    StatusMessage = "Пат! Ничья.";
                    break;
                case Models.Chess.Enums.GameState.Draw:
                    StatusMessage = "Ничья.";
                    break;
                default:
                    StatusMessage = $"Ход {_currentPlayer} игрока.";
                    break;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}