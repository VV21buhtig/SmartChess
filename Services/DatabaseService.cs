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
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<Game> CreateGameAsync(Game game)
        {
            return await _gameRepository.CreateGameAsync(game);
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
            return await _moveRepository.CreateMoveAsync(move);
        }

        public async Task<List<Move>> GetMovesByGameIdAsync(int gameId)
        {
            return await _moveRepository.GetMovesByGameIdAsync(gameId);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}