
namespace TraditionalBowlingDomain;
public record GameProgressResponseDto
{
    public List<string> FrameProgressScores { get; set; } = new(10);
    public bool GameCompleted => FrameProgressScores.Count == 10 && FrameProgressScores.Last() != "*";
}
