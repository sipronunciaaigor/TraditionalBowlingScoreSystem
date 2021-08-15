namespace TraditionalBowlingDomain;

public class SpareFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public SpareFrameScoreStrategy(List<int> pinsDowned, int index)
    {
        List<int> nextOne = pinsDowned.Skip(index).Take(1).ToList();
        int totScore = 10 + nextOne.Sum();
        ScoreLabel = new()
        {
            Score = totScore,
            UnknownLabel = nextOne.Count < 1
        };
    }

    public ScoreLabelDto GetScoreLabel()
    {
        return ScoreLabel;
    }
}