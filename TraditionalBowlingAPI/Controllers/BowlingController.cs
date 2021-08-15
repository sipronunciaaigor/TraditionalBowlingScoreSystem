using Microsoft.AspNetCore.Mvc;
using TraditionalBowlingDomain;
using TraditionalBowlingServices;

namespace TraditionalBowlingScoreSystem.Controllers;
[ApiController]
//[Route("[controller]")]
public class BowlingController : ControllerBase
{
    private readonly ILogger<BowlingController> _logger;

    public BowlingController(ILogger<BowlingController> logger)
    {
        _logger = logger;
    }

    [HttpPost("scores")]
    public IActionResult ComputeProgress(ScoreProgressRequestDto requestDto)
    {
        try
        {
            var frames = ScoringService.Frames.GetFrames(requestDto.PinsDowned);
            var scores = ScoringService.Scores.GetScores(requestDto.PinsDowned, frames);
            return Ok(scores);
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
