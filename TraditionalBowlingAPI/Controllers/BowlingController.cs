using Microsoft.AspNetCore.Mvc;
using TraditionalBowlingServices;

namespace TraditionalBowlingScoreSystem.Controllers;
[ApiController]
[Route("[controller]")]
public class BowlingController : ControllerBase
{
    private readonly ILogger<BowlingController> _logger;

    public BowlingController(ILogger<BowlingController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        List<int> input = new() { 1, 1, 1 };

        for (int i = 0; i < input.Count; i++)
        {
            Console.Write(string.Concat(i + 1, " [" + input[i] + "] -"));
        }
        Console.WriteLine(Environment.NewLine);

        var frames = ScoringService.Frames.GetFrames(input);
        for (int i = 0; i < frames.Count; i++)
        {
            Console.Write(string.Concat(i + 1, " [" + string.Join(',', frames[i]) + "] -"));
        }

        var scores = ScoringService.Scores.GetScores(input, frames);
        Console.WriteLine(string.Concat(Environment.NewLine, string.Join(',', scores)));

        return Ok(scores);
    }
}
