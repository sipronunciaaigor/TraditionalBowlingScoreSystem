namespace TraditionalBowlingDomain;

public class ClosedFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public ClosedFrameScoreStrategy(Frame frame)
    {
        ScoreLabel = new()
        {
            Score = frame.Sum
        };
    }

    public ScoreLabelDto GetScoreLabel()
    {
        return ScoreLabel;
    }
}