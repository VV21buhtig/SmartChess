using System.Windows.Controls;
using SmartChess.ViewModels;

namespace SmartChess.Views.UserControls
{
    public partial class GameHistoryView : UserControl
    {
        public GameHistoryView()
        {
            InitializeComponent();
            // Предположим, что HistoryViewModel передаётся как DataContext извне (например, из MainWindow)
            // DataContext = new HistoryViewModel(); // Не рекомендуется создавать VM внутри View
        }
    }
}