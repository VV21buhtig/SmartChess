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
        private int? _currentUserId; // Store user ID instead of depending on MainViewModel
        private ObservableCollection<Game> _games = new ObservableCollection<Game>();

        public HistoryViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadGamesCommand = new RelayCommand(async () => await LoadGamesAsync()); // ← исправлено
        }
        
        // Method to set current user ID (to be called from MainViewModel or elsewhere)
        public void SetCurrentUserId(int? userId)
        {
            _currentUserId = userId;
            // Optionally, reload games when user ID is set
            if (_currentUserId.HasValue)
            {
                _ = LoadGamesAsync();
            }
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
            // Загружаем игры текущего пользователя
            if (_currentUserId.HasValue)
            {
                var games = await _databaseService.GetGamesByUserIdAsync(_currentUserId.Value);
                Games = new ObservableCollection<Game>(games);
            }
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