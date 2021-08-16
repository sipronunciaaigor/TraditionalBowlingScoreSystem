namespace TraditionalBowlingDomain
{
    public class ClosedFrameScoreStrategy : IFrameScoreStrategy
    {
        ScoreLabelDto ScoreLabel { get; }

        public ClosedFrameScoreStrategy(int frameSum)
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
}