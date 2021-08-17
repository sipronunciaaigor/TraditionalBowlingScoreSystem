using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TraditionalBowlingDomain;
using TraditionalBowlingServices;

namespace TraditionalBowlingScoreSystem.Controllers
{
    [ApiController]
    public class BowlingController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<BowlingController> _logger;

        public BowlingController(IGameService gameService, ILogger<BowlingController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpPost("scores")]
        [ProducesResponseType(typeof(GameProgressResponseDto), StatusCodes.Status200OK)]
        public IActionResult Scores(ScoreProgressRequestDto requestDto)
        {
            try
            {
                return Ok(_gameService.GetScores(requestDto.PinsDowned));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}