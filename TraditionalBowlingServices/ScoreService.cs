using Microsoft.Extensions.Logging;
using TraditionalBowlingDomain;

namespace TraditionalBowlingServices;

public interface IScoreService
{
    List<string> GetScores(List<int> pinsDowned, List<List<int>> frames);
}

public class ScoreService : IScoreService
{
    private readonly ILogger<ScoreService> _logger;

    public ScoreService(ILogger<ScoreService> logger)
    {
        _logger = logger;
    }

    public List<string> GetScores(List<int> pinsDowned, List<Frame> frames)
    {
        List<string> labels = new();
        int score = 0;

        for (var i = 0; i < frames.Count; i++)
        {
            Frame frame = frames[i];
            frame.TopIndexInShotSequence += frame.Count;

            score = StrategyVisitor(score, labels,
                (frame.IsStrike(), new StrikeFrameScoreStrategy(frame, pinsDowned)),
                (frame.IsOpen(), new OpenFrameScoreStrategy(frame)),
                (frame.IsSpare(), new SpareFrameScoreStrategy(frame, pinsDowned)),
                (frame.IsClosed(), new ClosedFrameScoreStrategy(frame)),
                (frame.IsLatetst(), new LastFrameScoreStrategy(frame)));
        }

        return labels;
    }

    public static int StrategyVisitor(int score, List<string> labels,
        params (bool condition, IFrameScoreStrategy strategy)[] strategies)
    {
        var tuple = strategies.First(tuple => tuple.condition);
        IFrameScoreStrategy strategy = tuple.strategy;
        ScoreLabelDto scoreLabel = strategy.GetScoreLabel();
        score += scoreLabel.Score;
        labels.Add(scoreLabel.UnknownLabel ? "*" : score.ToString());
        return score;
    }
}