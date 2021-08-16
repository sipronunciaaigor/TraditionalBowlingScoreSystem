namespace TraditionalBowlingDomain;

public class StrikeFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public StrikeFrameScoreStrategy(Frame frame, List<int> pinsDowned)
    {
        List<int> nextTwo = pinsDowned.Skip(frame.AbsoluteLastIndex).Take(2).ToList();
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