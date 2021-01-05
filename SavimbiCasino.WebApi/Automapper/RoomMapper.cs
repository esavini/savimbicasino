using System;
using System.Collections.Generic;
using AutoMapper;
using SavimbiCasino.WebApi.Dtos;
using SavimbiCasino.WebApi.Models;

namespace SavimbiCasino.WebApi.Automapper
{
    public class RoomMapper : ITypeConverter<Tuple<Player, Bet, string>, PlayerDto>
    {
        public PlayerDto Convert(Tuple<Player, Bet, string> source, PlayerDto destination, ResolutionContext context)
        {
            var chips = new List<int>();

            if(source.Item2?.Amount is null)
            {
                chips = null;
            }
            else if (source.Item2.IsSplitted)
            {
                chips.Add(source.Item2.Amount.Value  / 2);
                chips.Add(source.Item2.Amount.Value / 2);
            }
            else
            {
                chips.Add(source.Item2.Amount.Value  / 2);
            }
            
            return new PlayerDto
            {
                Id = source.Item1.Id,
                Username = source.Item1.Username,
                Chips = chips
            };
        }
    }
}