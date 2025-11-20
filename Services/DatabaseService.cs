using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartChess.Data;
using SmartChess.Data.Repository;
using SmartChess.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IMoveRepository _moveRepository;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // Добавляем семафор для синхронизации доступа

        public DatabaseService(AppDbContext context, IUserRepository userRepository, IGameRepository gameRepository, IMoveRepository moveRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _moveRepository = moveRepository;
        }

        // --- Добавленный метод ---
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
        // --- Конец добавленного метода ---

        public async Task<User?> GetUserByLoginAsync(string login)
        {
            return await _userRepository.GetUserByLoginAsync(login);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user = await _userRepository.CreateUserAsync(user);
            await SaveChangesAsync();
            return user;
        }

        public async Task<Game> CreateGameAsync(Game game)
        {
            game = await _gameRepository.CreateGameAsync(game);
            await SaveChangesAsync();
            return game;
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _gameRepository.GetGameByIdAsync(id);
        }

        public async Task<List<Game>> GetGamesByUserIdAsync(int userId)
        {
            return await _gameRepository.GetGamesByUserIdAsync(userId);
        }

        public async Task<Move> CreateMoveAsync(Move move)
        {
            move = await _moveRepository.CreateMoveAsync(move);
            await SaveChangesAsync();
            return move;
        }

        public async Task<List<Move>> GetMovesByGameIdAsync(int gameId)
        {
            return await _moveRepository.GetMovesByGameIdAsync(gameId);
        }

        public async Task<int> SaveChangesAsync()
        {
            await _semaphore.WaitAsync(); // Ждем, пока не получим доступ
            try
            {
                return await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release(); // Освобождаем семафор
            }
        }

        public async Task UpdateGameAsync(Game game)
        {
            await _gameRepository.UpdateGameAsync(game);
            await SaveChangesAsync();
        }
    }
}