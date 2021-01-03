using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SavimbiCasino.WebApi.Dtos;
using SavimbiCasino.WebApi.Exceptions;
using SavimbiCasino.WebApi.Services;

namespace SavimbiCasino.WebApi.Controllers
{
    public class PlayerController : BaseApiController
    {
        private readonly IPlayerService _playerService;

        private readonly ILogger<PlayerController> _logger;

        public PlayerController(IPlayerService playerService, ILogger<PlayerController> logger)
        {
            _playerService = playerService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Login(CredentialsDto credentialsDto)
        {
            if (credentialsDto is null)
            {
                _logger.LogInformation("The {@DtoName} is null!", nameof(credentialsDto));
                throw new ArgumentNullException();
            }

            string token;

            try
            {
                token = await _playerService.LoginAsync(credentialsDto.Username, credentialsDto.Password);
            }
            catch (PlayerNotFoundException)
            {
                return NotFound();
            }
            catch (WrongPasswordException)
            {
                return Unauthorized();
            }

            var dto = new TokenDto
            {
                Token = token
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(CredentialsDto credentialsDto)
        {
            if (credentialsDto is null)
            {
                _logger.LogInformation("The {@DtoName} is null!", nameof(credentialsDto));
                throw new ArgumentNullException();
            }

            try
            {
                await _playerService.CreateAccountAsync(credentialsDto.Username, credentialsDto.Password);
            }
            catch (PlayerAlreadyExistsException)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}