using Microsoft.AspNetCore.Mvc;
using TraditionalBowlingDomain;
using TraditionalBowlingServices;

namespace TraditionalBowlingScoreSystem.Controllers;
[ApiController]
//[Route("[controller]")]
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
    public IActionResult ComputeProgress(ScoreProgressRequestDto requestDto)
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
