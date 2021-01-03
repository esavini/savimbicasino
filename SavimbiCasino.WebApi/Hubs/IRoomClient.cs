using System.Threading.Tasks;
using SavimbiCasino.WebApi.Dtos;

namespace SavimbiCasino.WebApi.Hubs
{
    public interface IRoomClient
    {
        Task UpdateRoom(RoomDto roomDto);
    }
}