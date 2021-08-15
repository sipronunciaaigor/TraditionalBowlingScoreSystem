namespace TraditionalBowlingDomain;

public class OpenFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public OpenFrameScoreStrategy(int frameSum)
    {
        ScoreLabel = new()
        {
            Score = frameSum,
            UnknownLabel = true
        };
    }

    public ScoreLabelDto GetScoreLabel()
    {
        return ScoreLabel;
    }
}