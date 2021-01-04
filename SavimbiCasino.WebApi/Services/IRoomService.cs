using System;
using System.Threading.Tasks;

namespace SavimbiCasino.WebApi.Services
{
    public interface IRoomService
    {
        /// <summary>
        /// Verifies if the room exists.
        /// </summary>
        /// <param name="guid">Room id.</param>
        /// <returns>True if exists, false otherwise.</returns>
        Task<bool> ExistsAsync(Guid guid);
    }
}