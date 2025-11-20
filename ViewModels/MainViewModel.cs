// ViewModels/MainViewModel.cs
using SmartChess.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SmartChess.Models.Entities;
using SmartChess.Views.UserControls;

namespace SmartChess.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        private User? _currentUser;

        // Изменён конструктор - принимает все VM
        public MainViewModel(AuthViewModel authViewModel, GameViewModel gameViewModel, HistoryViewModel historyViewModel, ProfileViewModel profileViewModel)
        {
            AuthViewModel = authViewModel;
            GameViewModel = gameViewModel;
            HistoryViewModel = historyViewModel;
            ProfileViewModel = profileViewModel;

            NavigateToGameCommand = new RelayCommand(() => CurrentView = GameViewModel);
            NavigateToHistoryCommand = new RelayCommand(() => CurrentView = HistoryViewModel);
            NavigateToProfileCommand = new RelayCommand(() => CurrentView = ProfileViewModel);
            NavigateToAuthCommand = new RelayCommand(() => CurrentView = AuthViewModel);


            // Подписываемся на события из AuthViewModel
            AuthViewModel.LoginSuccess += OnAuthLoginSuccess;
            AuthViewModel.RegistrationSuccess += OnAuthRegistrationSuccess;

            // Начинаем с экрана авторизации
            CurrentView = AuthViewModel;
        }

        public RelayCommand NavigateToGameCommand { get; }
        public RelayCommand NavigateToHistoryCommand { get; }
        public RelayCommand NavigateToProfileCommand { get; }
        public RelayCommand NavigateToAuthCommand { get; }

        public AuthViewModel AuthViewModel { get; }
        public GameViewModel GameViewModel { get; }
        public HistoryViewModel HistoryViewModel { get; }
        public ProfileViewModel ProfileViewModel { get; }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public User? CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        // Обработчики событий от AuthViewModel
        private void OnAuthLoginSuccess(User user)
        {
            CurrentUser = user;
            CurrentView = GameViewModel; // Переход к экрану игры
        }

        private void OnAuthRegistrationSuccess()
        {
            // Можно просто обновить сообщение в AuthViewModel или перейти к Login
            // AuthViewModel.Message = "Регистрация успешна! Теперь войдите.";
            // Или сразу переключить режим
            AuthViewModel.IsRegisterMode = false; // Переключаем на экран входа
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Методы для навигации (если нужно вызывать из других VM или Views)
        //public void NavigateToAuth() => CurrentView = AuthViewModel;
        //public void NavigateToGame() => CurrentView = GameViewModel;
        //public void NavigateToHistory() => CurrentView = HistoryViewModel;
        //public void NavigateToProfile() => CurrentView = ProfileViewModel;
    }
}