using System.Windows.Controls;
using SmartChess.ViewModels;

namespace SmartChess.Views.UserControls
{
    public partial class ChessBoardView : UserControl
    {
        public ChessBoardView()
        {
            InitializeComponent();
            // Предположим, что GameViewModel передаётся как DataContext извне (например, из MainWindow)
            // DataContext = new GameViewModel(); // Не рекомендуется создавать VM внутри View
        }
    }
}