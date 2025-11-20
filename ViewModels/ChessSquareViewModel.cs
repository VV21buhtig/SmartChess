//// Models/Chess/ChessSquareViewModel.cs
//using System.Windows.Media;
//using SmartChess.Models.Chess;

//namespace SmartChess.Models.Chess
//{
//    public class ChessSquareViewModel
//    {
//        public Position Position { get; set; }
//        public ChessPiece? Piece { get; set; }
//        public Brush BackgroundColor { get; set; }
//        //public string? PieceImagePath => Piece?.ImagePath;
//        public string? PieceImagePath
//        {
//            get
//            {
//                var path = Piece?.ImagePath;
//                System.Diagnostics.Trace.WriteLine($"=== IMAGE PATH for {Position}: {path} ===");
//                return path;
//            }
//        }
//    }
//}
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace SmartChess.Models.Chess
{
    public class ChessSquareViewModel : INotifyPropertyChanged
    {
        private Position _position;
        private ChessPiece? _piece;
        private Brush _backgroundColor;

        public Position Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        public ChessPiece? Piece
        {
            get => _piece;
            set
            {
                _piece = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PieceImagePath)); // Уведомляем об изменении ImagePath
            }
        }

        public Brush BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public string? PieceImagePath
        {
            get
            {
                var path = Piece?.ImagePath;
                System.Diagnostics.Trace.WriteLine($"=== IMAGE PATH for {Position}: {path} ===");
                return path;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}