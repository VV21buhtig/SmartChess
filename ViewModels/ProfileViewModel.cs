using SmartChess.Commands;
using SmartChess.Models.Entities;
using SmartChess.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartChess.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private User? _user;

        //public ProfileViewModel(DatabaseService databaseService) // Принимаем только DatabaseService
        //{
        //    _databaseService = databaseService;
        //}
        public ProfileViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadProfileCommand = new RelayCommand(async () => await LoadProfileAsync()); 
        }


        //public User? User
        //{
        //    get => _user;
        //    set
        //    {
        //        _user = value;
        //        OnPropertyChanged(nameof(User));
        //    }
        //}
        public User? User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        // МЕТОД LoadProfileCommand ТЕПЕРЬ ПРИНИМАЕТ ПАРАМЕТР - ID ПОЛЬЗОВАТЕЛЯ
        //public async void LoadProfileCommand(int userId)
        //{
        //    // Загружаем пользователя по переданному ID
        //    User = await _databaseService.GetUserByIdAsync(userId);
        //}
        public RelayCommand LoadProfileCommand { get; } 

        private async Task LoadProfileAsync() 
        {
            // Загружаем профиль текущего пользователя
            User = await _databaseService.GetUserByIdAsync(1); // Пока заглушка
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}