using System;
using AutoMapper;
using SavimbiCasino.WebApi.Dtos;
using SavimbiCasino.WebApi.Models;

namespace SavimbiCasino.WebApi.Automapper
{
    public class DealerAdminMapper : ITypeConverter<Tuple<Player, Bet, string>, BetDto>
    {
        public BetDto Convert(Tuple<Player, Bet, string> source, BetDto destination, ResolutionContext context)
        {
            return new BetDto
            {
                Username = source.Item1.Username,
                UserId = source.Item1.Id,
                Bet = source.Item2?.Amount ?? 0,
                IsDivided = source.Item2?.IsSplitted ?? false,
                IsDouble = source.Item2?.IsDouble ?? false
            };
        }
    }
}