using System;
using AutoMapper;
using SavimbiCasino.WebApi.Dtos;
using SavimbiCasino.WebApi.Models;

namespace SavimbiCasino.WebApi.Automapper
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<Tuple<Player, Bet, string>, PlayerDto>().ConvertUsing<RoomMapper>();
            CreateMap<Game, RoomDto>();
        }
    }
}