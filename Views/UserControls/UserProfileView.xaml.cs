using System.Windows.Controls;
using SmartChess.ViewModels;

namespace SmartChess.Views.UserControls
{
    public partial class UserProfileView : UserControl
    {
        public UserProfileView()
        {
            InitializeComponent();
            // Предположим, что ProfileViewModel передаётся как DataContext извне (например, из MainWindow)
            // DataContext = new ProfileViewModel(); // Не рекомендуется создавать VM внутри View
        }
    }
}