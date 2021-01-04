﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SavimbiCasino.WebApi.Enums;
using SavimbiCasino.WebApi.Exceptions;
using SavimbiCasino.WebApi.Models;
using SavimbiCasino.WebApi.Services;
using SavimbiCasino.WebApi.Dtos;

namespace SavimbiCasino.WebApi.Hubs
{
    public class RoomHub : Hub<IRoomClient>
    {
        private static IDictionary<Guid, Game> Games { get; } = new Dictionary<Guid, Game>();

        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        public RoomHub(IPlayerService playerService, IMapper mapper)
        {
            _playerService = playerService;
            _mapper = mapper;
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

            if (!Games.Keys.Contains(roomGuid))
            {
                throw new RoomNotFoundException();
            }
            
            var player = await _playerService.VerifyToken(token);

            if (player is null)
            {
                throw new InvalidTokenException();
            }

            if (Games[roomGuid].Players.Any(p => p.Item1.Id == player.Id))
            {
                throw new AlreadyInGameException();
            }
            
            Games[roomGuid].Players.Add(new Tuple<Player, Bet, string>(player, null, Context.ConnectionId));

            await UpdateRoom(roomGuid);
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

            if (Games.ContainsKey(roomGuid))
            {
                throw new RoomAlreadyInUseException();
            }

            // ToDo: verify the room exists in the db

            Games.Add(roomGuid, new Game
            {
                DealerConnectionId = Context.ConnectionId,
                Players = new List<Tuple<Player, Bet, string>>()
            });

            await UpdateRoom(roomGuid);
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

        private async Task UpdateRoom(Guid roomGuid)
        {
            var dto = _mapper.Map<RoomDto>(Games[roomGuid]);
            await Clients.Client(Games[roomGuid].DealerConnectionId).UpdateRoom(dto);
        }
    }
}