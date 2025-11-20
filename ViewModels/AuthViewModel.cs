using SmartChess.Commands; // RelayCommand
using SmartChess.Models.Entities;
using SmartChess.Services;
using SmartChess.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input; // Добавлено, если используется CommandManager

namespace SmartChess.ViewModels
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        // УБРАНО: private readonly MainViewModel _mainViewModel;
        private readonly AuthService _authService; // Предполагается, что инжектится
        private string _login = "";
        private string _password = "";
        private string _confirmPassword = "";
        private string _message = "";
        private bool _isRegisterMode = false;

        // ИЗМЕНЁН конструктор - убран MainViewModel
        public AuthViewModel(AuthService authService) // Принимаем только AuthService
        {
            _authService = authService;

            // ДОБАВЛЕНО: Инициализация команд в конструкторе
            RegisterCommand = new RelayCommand(RegisterCommandExecute);
            LoginCommand = new RelayCommand(LoginCommandExecute);
            NavigateToRegisterCommand = new RelayCommand(NavigateToRegister);
            NavigateToLoginCommand = new RelayCommand(NavigateToLogin);
        }

        public string Login
        {
            get => _login;
            set
            {
                System.Diagnostics.Trace.WriteLine($"Setting Login to: '{value}'");
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                System.Diagnostics.Trace.WriteLine($"=== PASSWORD SETTER CALLED - length: {value?.Length} ===");
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                System.Diagnostics.Trace.WriteLine($"=== CONFIRM PASSWORD SETTER CALLED - length: {value?.Length} ===");
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                System.Diagnostics.Trace.WriteLine($"message DataContext:  (length: {value?.Length})");
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public bool HasMessage => !string.IsNullOrEmpty(Message);

        public bool IsRegisterMode
        {
            get => _isRegisterMode;
            set
            {
                System.Diagnostics.Trace.WriteLine($"VALUE isrigistermode DataContext:  (length: {value}) ");
                _isRegisterMode = value;
                OnPropertyChanged(nameof(IsRegisterMode));
                CommandManager.InvalidateRequerySuggested(); // Пересчитываем CanExecute для команд
            }
        }

        // --- ИСПРАВЛЕННЫЕ КОМАНДЫ: объявлены как свойства с инициализацией в конструкторе ---
        public RelayCommand RegisterCommand { get; }
        public RelayCommand LoginCommand { get; }
        public RelayCommand NavigateToRegisterCommand { get; }
        public RelayCommand NavigateToLoginCommand { get; }

        // --- Методы Execute: НЕ принимают object ---
        private async void RegisterCommandExecute() // Убран object parameter
        {
            System.Diagnostics.Trace.WriteLine("=== REGISTER COMMAND EXECUTED ===");
            System.Diagnostics.Trace.WriteLine($"DEBUG - Login: '{Login}'");
            System.Diagnostics.Trace.WriteLine($"DEBUG - Password: '{(string.IsNullOrEmpty(Password) ? "EMPTY" : "***" + Password.Length + " chars")}'");
            System.Diagnostics.Trace.WriteLine($"DEBUG - Password length: {Password?.Length}");
            System.Diagnostics.Trace.WriteLine($"DEBUG - ConfirmPassword: '{(string.IsNullOrEmpty(ConfirmPassword) ? "EMPTY" : "***" + ConfirmPassword.Length + " chars")}'");
            System.Diagnostics.Trace.WriteLine($"DEBUG - ConfirmPassword length: {ConfirmPassword?.Length}");

            if (!Validator.IsValidLogin(Login))
            {
                System.Diagnostics.Trace.WriteLine("VALIDATION FAILED: Invalid login");
                Message = "Логин должен быть от 3 до 50 символов.";
                return;
            }

            if (!Validator.IsValidPassword(Password))
            {
                System.Diagnostics.Trace.WriteLine($"VALIDATION FAILED: Invalid password - Length: {Password?.Length}");
                Message = "Пароль должен быть не менее 6 символов.";
                return;
            }

            if (!Validator.ArePasswordsEqual(Password, ConfirmPassword))
            {
                System.Diagnostics.Trace.WriteLine("VALIDATION FAILED: Passwords don't match");
                Message = "Пароли не совпадают.";
                return;
            }
            System.Diagnostics.Trace.WriteLine("VALIDATION PASSED - CALLING AUTH SERVICE");
            try
            {
                bool success = await _authService.RegisterUserAsync(Login, Password);
                System.Diagnostics.Trace.WriteLine($"AUTH SERVICE RESULT: {success}");
                if (success)
                {
                    Message = "Регистрация успешна!";
                    // NavigateToLoginCommand.Execute(null); // Нельзя вызвать так, потому что NavigateToLogin ожидает RelayCommand без параметров
                    NavigateToLogin(); // Вызовем метод напрямую
                }
                else
                {
                    Message = "Пользователь с таким логином уже существует.";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"AUTH SERVICE ERROR: {ex.Message}");
                Message = $"Ошибка регистрации: {ex.Message}";
            }
        }

        // --- Методы Execute: НЕ принимают object ---
        private async void LoginCommandExecute() // Убран object parameter
        {
            System.Diagnostics.Trace.WriteLine("=== LOGIN COMMAND EXECUTED ===");
            System.Diagnostics.Trace.WriteLine($"DEBUG - Login: '{Login}'");
            System.Diagnostics.Trace.WriteLine($"DEBUG - Password: '{(string.IsNullOrEmpty(Password) ? "EMPTY" : "***" + Password.Length + " chars")}'");
            System.Diagnostics.Trace.WriteLine($"DEBUG - Password length: {Password?.Length}");

            if (!Validator.IsValidLogin(Login))
            {
                Message = "Логин должен быть от 3 до 50 символов.";
                return;
            }

            if (!Validator.IsValidPassword(Password))
            {
                Message = "Пароль должен быть не менее 6 символов.";
                return;
            }

            var user = await _authService.AuthenticateUserAsync(Login, Password);
            if (user != null)
            {
                // Уведомляем MainViewModel об успешном входе через событие (если MainViewModel подписан)
                OnLoginSuccess(user);
            }
            else
            {
                Message = "Неверный логин или пароль.";
            }
        }

        // --- Методы Execute для навигации: НЕ принимают object ---
        private void NavigateToRegister() // Убран object parameter
        {
            IsRegisterMode = true;
        }

        private void NavigateToLogin() // Убран object parameter
        {
            IsRegisterMode = false;
        }

        // --- СОБЫТИЯ для уведомления MainViewModel (если MainViewModel подписан) ---
        public event Action<User>? LoginSuccess;
        public event Action? RegistrationSuccess;

        protected virtual void OnLoginSuccess(User user) => LoginSuccess?.Invoke(user);
        protected virtual void OnRegistrationSuccess() => RegistrationSuccess?.Invoke();
        // --- КОНЕЦ СОБЫТИЙ ---

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}