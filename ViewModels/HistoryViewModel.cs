using SmartChess.Commands;
using SmartChess.Models.Entities;
using SmartChess.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartChess.ViewModels
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;
        private ObservableCollection<Game> _games = new ObservableCollection<Game>();

        //public HistoryViewModel()
        //{
        //    _databaseService = new DatabaseService(new Data.AppDbContext(), new Data.Repository.UserRepository(new Data.AppDbContext()), new Data.Repository.GameRepository(new Data.AppDbContext()), new Data.Repository.MoveRepository(new Data.AppDbContext()));
        //}
        public HistoryViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadGamesCommand = new RelayCommand(async () => await LoadGamesAsync()); // ← исправлено
        }


        //public ObservableCollection<Game> Games
        //{
        //    get => _games;
        //    set
        //    {
        //        _games = value;
        //        OnPropertyChanged(nameof(Games));
        //    }
        //}
        public ObservableCollection<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoadGamesCommand { get; }

        private async Task LoadGamesAsync() // ← ДОБАВЬ private
        {
            // Загружаем игры текущего пользователя (нужно передать userId)
            var games = await _databaseService.GetGamesByUserIdAsync(1); // Пока заглушка
            Games = new ObservableCollection<Game>(games);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}