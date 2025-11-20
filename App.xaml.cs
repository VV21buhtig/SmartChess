using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartChess.Data;
using SmartChess.Data.Repository;
using SmartChess.Services;
using SmartChess.ViewModels;
using SmartChess.Views;
using SmartChess.Algorithms;
using System;
using System.Windows;
using System.Windows.Threading;

namespace SmartChess
{
    public partial class App : Application
    {
        private IHost? _host;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            _host = Host.CreateDefaultBuilder(e.Args)
                .ConfigureServices((context, services) =>
                {
                    // Конфигурация
                    services.AddSingleton<IConfiguration>(context.Configuration);

                    // База данных
                    services.AddDbContext<AppDbContext>();
                    services.AddScoped<IUserRepository, UserRepository>();
                    services.AddScoped<IGameRepository, GameRepository>();
                    services.AddScoped<IMoveRepository, MoveRepository>();
                    services.AddScoped<DatabaseService>();

                    // Сервисы
                    services.AddScoped<AuthService>();
                    services.AddScoped<IChessEngine, ChessEngine>();
                    services.AddScoped<GameSessionService>();
                    services.AddScoped<AlgorithmService>();
                    services.AddScoped<PathFinder>();
                    services.AddScoped<GameAnalyzer>();
                    services.AddScoped<MoveCalculator>();

                    // ViewModels
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<GameViewModel>();
                    services.AddTransient<HistoryViewModel>();
                    services.AddTransient<ProfileViewModel>();
                    services.AddTransient<AuthViewModel>();


                    // Views
                    services.AddTransient<MainWindow>();
                })
                .Build();

            await EnsureAdminUserExists();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private async Task EnsureAdminUserExists()
        {
            using var scope = _host.Services.CreateScope();
            var dbService = scope.ServiceProvider.GetRequiredService<DatabaseService>();

            var existingAdmin = await dbService.GetUserByLoginAsync("admin");
            if (existingAdmin == null)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123");
                var adminUser = new Models.Entities.User
                {
                    Login = "admin",
                    PasswordHash = passwordHash
                };

                await dbService.CreateUserAsync(adminUser);
                await dbService.SaveChangesAsync(); // Сохранить изменения в БД
                Console.WriteLine("Администратор 'admin' создан с паролем 'admin123'.");
            }
            else
            {
                Console.WriteLine("Администратор 'admin' уже существует.");
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host is not null)
            {
                await _host.StopAsync();
            }
            base.OnExit(e);
        }
    }
}