using System;
using System.Threading.Tasks;
using SavimbiCasino.WebApi.Exceptions;

namespace SavimbiCasino.WebApi.Services
{
    /// <summary>
    /// Player primitives.
    /// </summary>
    public interface IPlayerService
    {
        /// <summary>
        /// When invoked checks user credentials.
        /// </summary>
        /// <param name="username">Player's username.</param>
        /// <param name="password">Player's cleartext password.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="username"/> or <paramref name="password"/> is null.</exception>
        /// <exception cref="ArgumentException">When <paramref name="username"/> or <paramref name="password"/> is empty or whitespace.</exception>
        /// <exception cref="PlayerNotFoundException">When player is not found.</exception>
        /// <exception cref="WrongPasswordException">When the password is not found.</exception>
        /// <returns>The session token.</returns>
        Task<string> LoginAsync(string username, string password);

        /// <summary>
        /// When invoked creates a new account.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <exception cref="ArgumentNullException">When <paramref name="username"/> or <paramref name="password"/> is null.</exception>
        /// <exception cref="ArgumentException">When <paramref name="username"/> or <paramref name="password"/> is empty or whitespace.</exception>
        /// <exception cref="PlayerAlreadyExistsException">When a player with the same <paramref name="username"/> already exists.</exception>
        /// <returns>The task.</returns>
        Task CreateAccountAsync(string username, string password);
    }
}