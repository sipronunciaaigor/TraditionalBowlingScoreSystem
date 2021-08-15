namespace TraditionalBowlingDomain;

public class LastFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public LastFrameScoreStrategy(int frameSum)
    {
        ScoreLabel = new()
        {
            Score = frameSum
        };
    }

    public ScoreLabelDto GetScoreLabel()
    {
        return ScoreLabel;
    }
}