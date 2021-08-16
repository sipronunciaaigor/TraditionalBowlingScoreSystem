namespace TraditionalBowlingDomain;

public class FrameVisitor
{
    private List<Frame> _frames = new();

    public void Execute(
        List<string> labels,
        List<Frame> frames,
        params (bool condition, IFrameScoreStrategy strategy)[] strategies)
    {
        int score = 0;

        for (var i = 0; i < frames.Count; i++)
        {
            frames[i].TopIndexInShotSequence += frames[i].Count;

            foreach (var strategy in strategies)
            {
                if (strategy.condition)
                {
                    ScoreLabelDto scoreLabel = strategy.strategy.GetScoreLabel();
                    score += scoreLabel.Score;
                    labels.Add(scoreLabel.UnknownLabel ? "*" : score.ToString());
                }
            }
        }
    }
    
    public void Execut2e(
        List<string> labels,
        List<Frame> frames,
        params Action<FrameVisitor, Frame> []strategies)
    {
        int score = 0;

        for (var i = 0; i < frames.Count; i++)
        {
            frames[i].TopIndexInShotSequence += frames[i].Count;

            foreach (var strategy in strategies)
            {
                if (strategy.condition)
                {
                    ScoreLabelDto scoreLabel = strategy.strategy.GetScoreLabel();
                    score += scoreLabel.Score;
                    labels.Add(scoreLabel.UnknownLabel ? "*" : score.ToString());
                }
            }
        }
    }
}