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
        List<int> input = new(21);
        input = new() { 1, 1, 1 };
        // input = new() { 10, 1, 1 };
        // input = new() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        // input = new() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        // input = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // input = new() { };
        // input = new() { 1, 1, 1 };
        // input = new() {10, 1, 1};
        //input = new() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        // input = new() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        input = new() { 9, 1, 1, 9, 9, 1, 9, 9, 9, 9, 9, 9, 9 };
        input = new() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
        // input = new() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 9, 9 };
        input = new() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 9, 1, 9 }; // err > last 2 must be ? ?
        input = new() { 10, 10, 10, 10, 10, 10, 10, 10, 10, 9, 0, 1 };
        //input = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        input = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        input = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // input = new() { };

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
