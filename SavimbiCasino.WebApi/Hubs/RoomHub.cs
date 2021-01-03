using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using SavimbiCasino.WebApi.Enums;
using SavimbiCasino.WebApi.Exceptions;
using SavimbiCasino.WebApi.Models;
using SavimbiCasino.WebApi.Services;
using System.Text.Json;
using SavimbiCasino.WebApi.Dtos;

namespace SavimbiCasino.WebApi.Hubs
{
    public class RoomHub : Hub<IRoomClient>
    {
        private readonly IDictionary<Guid, Game> _games;

        private readonly IServiceProvider _serviceProvider;

        public RoomHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _games = new Dictionary<Guid, Game>();
        }

        public async Task Login(string token, string roomId)
        {
            #region Params checks

            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(nameof(token));
            }

            if (roomId is null)
            {
                throw new ArgumentNullException(nameof(roomId));
            }

            if (string.IsNullOrWhiteSpace(roomId))
            {
                throw new ArgumentException(nameof(roomId));
            }

            #endregion

            bool parsed = Guid.TryParse(roomId, out var roomGuid);

            if (!parsed)
            {
                throw new ArgumentException(nameof(roomId));
            }

            if (!_games.Keys.Contains(roomGuid))
            {
                throw new RoomNotFoundException();
            }

            var playerService = _serviceProvider.GetRequiredService<IPlayerService>();

            var player = await playerService.VerifyToken(token);

            if (player is null)
            {
                throw new InvalidTokenException();
            }

            // ToDo
        }

        public async Task DealerJoin(string roomId)
        {
            if (roomId is null)
            {
                throw new ArgumentNullException(nameof(roomId));
            }

            bool parsed = Guid.TryParse(roomId, out var roomGuid);

            if (!parsed)
            {
                throw new ArgumentException(nameof(roomId));
            }

            if (_games.ContainsKey(roomGuid))
            {
                throw new RoomAlreadyInUseException();
            }

            // ToDo: verify the room exists in the db

            _games[roomGuid] = new Game
            {
                DealerConnectionId = Context.ConnectionId,
                Bets = new List<Bet>()
            };

            await Clients.Client(_games[roomGuid].DealerConnectionId).UpdateRoom(new RoomDto
            {
                Players = new []
                {
                    new PlayerDto
                    {
                        Id = Guid.NewGuid(),
                        Username = "Savimbi",
                        Chips = null
                    }
                }
            });
        }

        public async Task Bet(int amount)
        {
        }

        public async Task PerformGameAction(GameAction gameAction)
        {
        }

        public async Task ChangeGameStatus()
        {
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}