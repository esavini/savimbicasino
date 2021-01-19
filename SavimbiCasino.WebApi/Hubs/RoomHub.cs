using System;
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
            await UpdatePlayer(Games[roomGuid], player);
            await UpdateDealerAdmin(roomGuid);
        }

        public async Task DealerAdminJoin(string roomId)
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

            if (!Games.ContainsKey(roomGuid))
            {
                throw new RoomNotFoundException();
            }

            if (Games[roomGuid].DealerAdminConnectionId != null)
            {
                throw new RoomAlreadyInUseException();
            }

            Games[roomGuid].DealerAdminConnectionId = Context.ConnectionId;

            await UpdateDealerAdmin(roomGuid);
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

            Games.Add(roomGuid, new Game
            {
                Status = GameStatus.Idle,
                DealerConnectionId = Context.ConnectionId,
                Players = new List<Tuple<Player, Bet, string>>()
            });

            await UpdateDealerAdmin(roomGuid);
            await UpdateRoom(roomGuid);
        }

        public async Task ChangeGameStatus(GameStatus status)
        {
            var dealerGame = Games.Where((elem) => elem.Value.DealerAdminConnectionId == Context.ConnectionId)
                .Select(elem => Tuple.Create(elem.Key, elem.Value)).FirstOrDefault();

            if (dealerGame is null)
            {
                throw new ArgumentException();
            }

            if (status == GameStatus.Idle)
            {
                for (var i = 0; i < Games[dealerGame.Item1].Players.Count; i++)
                {
                    var player = Games[dealerGame.Item1].Players[i].Item1;
                    var connId = Games[dealerGame.Item1].Players[i].Item3;

                    Games[dealerGame.Item1].Players[i] = Tuple.Create<Player, Bet, string>(player, null, connId);
                }
            }

            Games[dealerGame.Item1].Status = status;

            foreach (var player in Games[dealerGame.Item1].Players)
            {
                await UpdatePlayer(Games[dealerGame.Item1], player.Item1, player.Item2);
            }

            await UpdateDealerAdmin(dealerGame.Item1);
            await UpdateRoom(dealerGame.Item1);
        }

        public async Task WinDouble(string userId)
        {
            var userGuid = Guid.Parse(userId);

            var game = Games.First(game => game.Value.Players.Any(p => p.Item1.Id == userGuid));

            if (game.Value.Status != GameStatus.Playing)
            {
                return;
            }

            for (int i = 0; i < Games[game.Key].Players.Count; i++)
            {
                if (Games[game.Key].Players[i].Item1.Id != userGuid)
                {
                    continue;
                }

                if (Games[game.Key].Players[i].Item2 is null)
                {
                    return;
                }

                var bet = Games[game.Key].Players[i].Item2.Amount ?? 0;

                if (bet == 0)
                {
                    return;
                }

                await _playerService.IncrementMoney(Games[game.Key].Players[i].Item1, bet * 2);
                Games[game.Key].Players[i].Item1.Money =
                    await _playerService.GetMoney(Games[game.Key].Players[i].Item1);

                Games[game.Key].Players[i] = Tuple.Create<Player, Bet, string>(Games[game.Key].Players[i].Item1, null,
                    Games[game.Key].Players[i].Item3);

                await UpdatePlayer(Games[game.Key], Games[game.Key].Players[i].Item1, Games[game.Key].Players[i].Item2);

                break;
            }

            await UpdateDealerAdmin(game.Key);
            await UpdateRoom(game.Key);
        }
        
        public async Task Push(string userId)
        {
            var userGuid = Guid.Parse(userId);

            var game = Games.First(game => game.Value.Players.Any(p => p.Item1.Id == userGuid));

            if (game.Value.Status != GameStatus.Playing)
            {
                return;
            }

            for (int i = 0; i < Games[game.Key].Players.Count; i++)
            {
                if (Games[game.Key].Players[i].Item1.Id != userGuid)
                {
                    continue;
                }

                if (Games[game.Key].Players[i].Item2 is null)
                {
                    return;
                }

                var bet = Games[game.Key].Players[i].Item2.Amount ?? 0;

                if (bet == 0)
                {
                    return;
                }

                await _playerService.IncrementMoney(Games[game.Key].Players[i].Item1, bet);
                Games[game.Key].Players[i].Item1.Money =
                    await _playerService.GetMoney(Games[game.Key].Players[i].Item1);

                Games[game.Key].Players[i] = Tuple.Create<Player, Bet, string>(Games[game.Key].Players[i].Item1, null,
                    Games[game.Key].Players[i].Item3);

                await UpdatePlayer(Games[game.Key], Games[game.Key].Players[i].Item1, Games[game.Key].Players[i].Item2);

                break;
            }

            await UpdateDealerAdmin(game.Key);
            await UpdateRoom(game.Key);
        }

        public async Task Blackjack(string userId)
        {
            var userGuid = Guid.Parse(userId);

            var game = Games.First(game => game.Value.Players.Any(p => p.Item1.Id == userGuid));

            if (game.Value.Status != GameStatus.Playing)
            {
                return;
            }

            for (int i = 0; i < Games[game.Key].Players.Count; i++)
            {
                if (Games[game.Key].Players[i].Item1.Id != userGuid)
                {
                    continue;
                }

                if (Games[game.Key].Players[i].Item2 is null)
                {
                    return;
                }

                var bet = Games[game.Key].Players[i].Item2.Amount ?? 0;

                if (bet == 0)
                {
                    return;
                }

                if (Games[game.Key].Players[i].Item2.IsSplitted || Games[game.Key].Players[i].Item2.IsDouble)
                {
                    return;
                }

                bet = (int) (bet * 2 * 1.5);

                await _playerService.IncrementMoney(Games[game.Key].Players[i].Item1, bet);
                Games[game.Key].Players[i].Item1.Money =
                    await _playerService.GetMoney(Games[game.Key].Players[i].Item1);

                Games[game.Key].Players[i] = Tuple.Create<Player, Bet, string>(Games[game.Key].Players[i].Item1, null,
                    Games[game.Key].Players[i].Item3);

                await UpdatePlayer(Games[game.Key], Games[game.Key].Players[i].Item1, Games[game.Key].Players[i].Item2);

                break;
            }

            await UpdateDealerAdmin(game.Key);
            await UpdateRoom(game.Key);
        }

        public async Task Charge(string userId)
        {
            var userGuid = Guid.Parse(userId);

            var game = Games.First(game => game.Value.Players.Any(p => p.Item1.Id == userGuid));

            for (int i = 0; i < Games[game.Key].Players.Count; i++)
            {
                if (Games[game.Key].Players[i].Item1.Id != userGuid)
                {
                    continue;
                }

                await _playerService.IncrementMoney(Games[game.Key].Players[i].Item1, 25);

                await UpdatePlayer(Games[game.Key], Games[game.Key].Players[i].Item1, Games[game.Key].Players[i].Item2);

                break;
            }
        }

        public async Task Kick(string userId)
        {
            var userGuid = Guid.Parse(userId);

            var game = Games.First(game => game.Value.Players.Any(p => p.Item1.Id == userGuid));

            for (int i = 0; i < Games[game.Key].Players.Count; i++)
            {
                if (Games[game.Key].Players[i].Item1.Id != userGuid)
                {
                    continue;
                }

                await Clients.Client(Games[game.Key].Players[i].Item3).RoomClosed();

                break;
            }

            await UpdateDealerAdmin(game.Key);
            await UpdateRoom(game.Key);
        }

        public async Task Win(string userId)
        {
            var userGuid = Guid.Parse(userId);

            var game = Games.First(game => game.Value.Players.Any(p => p.Item1.Id == userGuid));

            if (game.Value.Status != GameStatus.Playing)
            {
                return;
            }

            for (int i = 0; i < Games[game.Key].Players.Count; i++)
            {
                if (Games[game.Key].Players[i].Item1.Id != userGuid)
                {
                    continue;
                }

                if (Games[game.Key].Players[i].Item2 is null)
                {
                    return;
                }

                var bet = Games[game.Key].Players[i].Item2.Amount ?? 0;

                if (bet == 0)
                {
                    return;
                }

                if (Games[game.Key].Players[i].Item2.IsSplitted)
                {
                    bet = bet / 2;
                }

                await _playerService.IncrementMoney(Games[game.Key].Players[i].Item1, bet * 2);
                Games[game.Key].Players[i].Item1.Money =
                    await _playerService.GetMoney(Games[game.Key].Players[i].Item1);

                Games[game.Key].Players[i] = Tuple.Create<Player, Bet, string>(Games[game.Key].Players[i].Item1, null,
                    Games[game.Key].Players[i].Item3);

                await UpdatePlayer(Games[game.Key], Games[game.Key].Players[i].Item1, Games[game.Key].Players[i].Item2);

                break;
            }

            await UpdateDealerAdmin(game.Key);
            await UpdateRoom(game.Key);
        }

        public async Task Bet(int amount)
        {
            var game = Games.First(game => game.Value.Players.Any(p => p.Item3 == Context.ConnectionId));

            if (game.Value.Status != GameStatus.WaitingForBets)
            {
                return;
            }

            for (var i = 0; i < Games[game.Key].Players.Count(); i++)
            {
                if (Games[game.Key].Players[i].Item3 == Context.ConnectionId)
                {
                    if (!await _playerService.VerifyMoney(Games[game.Key].Players[i].Item1, amount))
                    {
                        return;
                    }

                    var decreaseTask = _playerService.DecreaseMoney(Games[game.Key].Players[i].Item1, amount);

                    if (Games[game.Key].Players[i].Item2 is null)
                    {
                        Games[game.Key].Players[i] = Tuple.Create(Games[game.Key].Players[i].Item1, new Bet
                        {
                            Amount = 0
                        }, Games[game.Key].Players[i].Item3);
                    }

                    Games[game.Key].Players[i].Item2.Amount += amount;

                    await decreaseTask;

                    await UpdatePlayer(Games[game.Key], Games[game.Key].Players[i].Item1,
                        Games[game.Key].Players[i].Item2);

                    break;
                }
            }

            await UpdateDealerAdmin(game.Key);
            await UpdateRoom(game.Key);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;

            var dealerGame = Games.Where((elem) => elem.Value.DealerConnectionId == connectionId)
                .Select(elem => Tuple.Create(elem.Key, elem.Value)).FirstOrDefault();

            if (dealerGame != null)
            {
                await CloseGame(dealerGame.Item1);
                return;
            }

            var dealerAdminGame = Games.Where((elem) => elem.Value.DealerAdminConnectionId == connectionId)
                .Select(elem => Tuple.Create(elem.Key, elem.Value)).FirstOrDefault();

            if (dealerAdminGame != null)
            {
                Games[dealerAdminGame.Item1].DealerAdminConnectionId = null;
            }

            var playerGame = Games.Where((elem) =>
                    elem.Value.Players.Any(p => p.Item3 == connectionId))
                .Select(elem => Tuple.Create(elem.Key, elem.Value)).FirstOrDefault();

            if (playerGame != null)
            {
                var player = playerGame.Item2.Players.First(p => p.Item3 == connectionId);
                Games[playerGame.Item1].Players.Remove(player);
                await UpdateDealerAdmin(playerGame.Item1);
                await UpdateRoom(playerGame.Item1);
            }
        }

        private async Task UpdateRoom(Guid roomGuid)
        {
            var dto = _mapper.Map<RoomDto>(Games[roomGuid]);
            await Clients.Client(Games[roomGuid].DealerConnectionId).UpdateRoom(dto);
        }

        private async Task UpdateDealerAdmin(Guid roomGuid)
        {
            var dto = _mapper.Map<DealerAdminDto>(Games[roomGuid]);

            if (Games[roomGuid].DealerAdminConnectionId != null)
            {
                await Clients.Client(Games[roomGuid].DealerAdminConnectionId).UpdateDealerAdmin(dto);
            }
        }

        private async Task CloseGame(Guid roomId)
        {
            foreach (var player in Games[roomId].Players)
            {
                await Clients.Client(player.Item3).RoomClosed();
            }

            if (Games[roomId].DealerAdminConnectionId != null)
            {
                await Clients.Client(Games[roomId].DealerAdminConnectionId).RoomClosed();
            }

            Games.Remove(roomId);
        }

        public async Task Divide()
        {
            var game = Games.First(game => game.Value.Players.Any(p => p.Item3 == Context.ConnectionId));

            if (game.Value.Status != GameStatus.Playing)
            {
                return;
            }

            for (var i = 0; i < Games[game.Key].Players.Count(); i++)
            {
                if (Games[game.Key].Players[i].Item3 == Context.ConnectionId)
                {
                    if (Games[game.Key].Players[i].Item2?.Amount is null)
                    {
                        return;
                    }

                    if (Games[game.Key].Players[i].Item2.IsDouble || Games[game.Key].Players[i].Item2.IsSplitted)
                    {
                        return;
                    }

                    if (!await _playerService.VerifyMoney(Games[game.Key].Players[i].Item1,
                        Games[game.Key].Players[i].Item2.Amount.Value))
                    {
                        return;
                    }

                    var decreaseTask = _playerService.DecreaseMoney(Games[game.Key].Players[i].Item1,
                        Games[game.Key].Players[i].Item2.Amount.Value);

                    Games[game.Key].Players[i].Item2.Amount += Games[game.Key].Players[i].Item2.Amount.Value;
                    Games[game.Key].Players[i].Item2.IsSplitted = true;


                    await decreaseTask;

                    await UpdatePlayer(Games[game.Key], Games[game.Key].Players[i].Item1,
                        Games[game.Key].Players[i].Item2);

                    break;
                }
            }

            await UpdateDealerAdmin(game.Key);
            await UpdateRoom(game.Key);
        }

        public async Task Double()
        {
            var game = Games.First(game => game.Value.Players.Any(p => p.Item3 == Context.ConnectionId));

            if (game.Value.Status != GameStatus.Playing)
            {
                return;
            }

            for (var i = 0; i < Games[game.Key].Players.Count(); i++)
            {
                if (Games[game.Key].Players[i].Item3 == Context.ConnectionId)
                {
                    if (Games[game.Key].Players[i].Item2?.Amount is null)
                    {
                        return;
                    }


                    if (Games[game.Key].Players[i].Item2.IsDouble || Games[game.Key].Players[i].Item2.IsSplitted)
                    {
                        return;
                    }

                    if (!await _playerService.VerifyMoney(Games[game.Key].Players[i].Item1,
                        Games[game.Key].Players[i].Item2.Amount.Value))
                    {
                        return;
                    }

                    var decreaseTask = _playerService.DecreaseMoney(Games[game.Key].Players[i].Item1,
                        Games[game.Key].Players[i].Item2.Amount.Value);

                    Games[game.Key].Players[i].Item2.Amount += Games[game.Key].Players[i].Item2.Amount.Value;
                    Games[game.Key].Players[i].Item2.IsDouble = true;

                    await decreaseTask;

                    await UpdatePlayer(Games[game.Key], Games[game.Key].Players[i].Item1,
                        Games[game.Key].Players[i].Item2);

                    break;
                }
            }

            await UpdateDealerAdmin(game.Key);
            await UpdateRoom(game.Key);
        }

        private async Task UpdatePlayer(Game game, Player player, Bet bet = null)
        {
            bool canBet = game.Status == GameStatus.WaitingForBets;
            bool canDivide = true;
            bool canDouble = true;

            if (bet is null)
            {
                canDivide = false;
                canDouble = false;
            }
            else if (bet.IsDouble || bet.IsSplitted)
            {
                canDivide = false;
                canDouble = false;
            }

            if (game.Status != GameStatus.Playing)
            {
                canDivide = false;
                canDouble = false;
            }

            var money = await _playerService.GetMoney(player);

            await Clients.Client(game.Players.First(p => p.Item1.Id == player.Id).Item3).UpdatePlayer(new PlayerGameDto
            {
                Money = money,
                GameStatus = game.Status,
                CanBet = canBet,
                CanDivide = canDivide,
                CanDouble = canDouble
            });
        }
    }
}