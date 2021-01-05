using System.Threading.Tasks;
using SavimbiCasino.WebApi.Dtos;

namespace SavimbiCasino.WebApi.Hubs
{
    public interface IRoomClient
    {
        Task UpdateRoom(RoomDto roomDto);

        Task UpdatePlayer(PlayerGameDto playerGameDto);
        
        Task UpdateDealerAdmin(DealerAdminDto dealerGameDto);

        Task RoomClosed();
    }
}