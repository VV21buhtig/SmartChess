// Views/UserControls/RegisterView.xaml.cs
using System.Windows.Controls;
using SmartChess.ViewModels;

namespace SmartChess.Views.UserControls
{
    public partial class RegisterView : UserControl
    {
        private readonly AuthViewModel? _viewModel; // Сделаем поле nullable

        // Конструктор без параметров (для WPF)
        public RegisterView()
        {
            InitializeComponent();
            // В этом случае DataContext может быть установлен позже через DataTemplate
        }

        // Конструктор с параметром (для ручного создания)
        public RegisterView(AuthViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
}