using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SavimbiCasino.WebApi.Exceptions;
using SavimbiCasino.WebApi.Models;

namespace SavimbiCasino.WebApi.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly SavimbiCasinoDbContext _dbContext;

        public PlayerService(SavimbiCasinoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc cref="IPlayerService.LoginAsync"/>
        public async Task<string> LoginAsync(string username, string password)
        {
            if (username is null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

            Player player = await _dbContext.Players.FirstOrDefaultAsync(p => p.Username == username);

            if (player is null)
            {
                throw new PlayerNotFoundException();
            }

            if (!BCrypt.Net.BCrypt.Verify(password, player.HashedPassword))
            {
                throw new WrongPasswordException();
            }

            var token = Guid.NewGuid().ToString();

            player.SessionToken = token;

            _dbContext.Update(player);
            await _dbContext.SaveChangesAsync();

            return token;
        }

        /// <inheritdoc cref="IPlayerService.CreateAccountAsync"/>
        public async Task CreateAccountAsync(string username, string password)
        {
            if (username is null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException(nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException(nameof(password));
            }

            Player player = await _dbContext.Players.FirstOrDefaultAsync(p => p.Username == username);

            if (player != null)
            {
                throw new PlayerAlreadyExistsException();
            }

            player = new Player
            {
                Username = username,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _dbContext.AddAsync(player);
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc cref="IPlayerService.VerifyToken"/>
        public async Task<Player> VerifyToken(string token)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(nameof(token));
            }

            return await _dbContext.Players.FirstOrDefaultAsync(p => p.SessionToken == token);
        }
    }
}