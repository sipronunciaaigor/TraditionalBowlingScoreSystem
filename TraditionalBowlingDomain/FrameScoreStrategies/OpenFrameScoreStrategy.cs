namespace TraditionalBowlingDomain;

public class OpenFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public OpenFrameScoreStrategy(Frame frame)
    {
        ScoreLabel = new()
        {
            Score = frame.Sum,
            UnknownLabel = true
        };
    }

    public ScoreLabelDto GetScoreLabel()
    {
        return ScoreLabel;
    }
}