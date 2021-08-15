namespace TraditionalBowlingDomain;

public class StrikeFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public StrikeFrameScoreStrategy(List<int> pinsDowned, int index)
    {
        List<int> nextTwo = pinsDowned.Skip(index).Take(2).ToList();
        int totScore = 10 + nextTwo.Sum();
        ScoreLabel = new()
        {
            Score = totScore,
            UnknownLabel = nextTwo.Count < 2
        };
    }

    public ScoreLabelDto GetScoreLabel()
    {
        return ScoreLabel;
    }
}