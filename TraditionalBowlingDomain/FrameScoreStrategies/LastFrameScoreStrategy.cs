namespace TraditionalBowlingDomain;

public class LastFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public LastFrameScoreStrategy(Frame frame)
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