using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SavimbiCasino.WebApi.Services
{
    public class RoomService : IRoomService
    {
        private readonly SavimbiCasinoDbContext _dbContext;

        public RoomService(SavimbiCasinoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <inheritdoc cref="IRoomService.ExistsAsync"/>
        public async Task<bool> ExistsAsync(Guid guid)
        {
            return await _dbContext.Rooms.AnyAsync(room => room.Id == guid);
        }
    }
}