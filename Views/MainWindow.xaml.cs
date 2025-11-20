// Views/MainWindow.xaml.cs
using System.Windows;
using SmartChess.ViewModels;

namespace SmartChess.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel; // Добавьте поле для хранения MainViewModel

        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            _mainViewModel = mainViewModel;
            DataContext = _mainViewModel;
            //_mainViewModel.NavigateToAuth(); // Вызываем метод навигации
        }

        //public void NavigateToAuth()
        //{
        //    _mainViewModel.NavigateToAuth();
        //}
    }
}