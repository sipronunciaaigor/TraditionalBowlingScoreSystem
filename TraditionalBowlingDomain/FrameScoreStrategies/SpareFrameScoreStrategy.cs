namespace TraditionalBowlingDomain;

public class SpareFrameScoreStrategy : IFrameScoreStrategy
{
    ScoreLabelDto ScoreLabel { get; }

    public SpareFrameScoreStrategy(Frame frame, List<int> pinsDowned)
    {
        List<int> nextOne = pinsDowned.Skip(frame.TopIndexInShotSequence).Take(1).ToList();
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