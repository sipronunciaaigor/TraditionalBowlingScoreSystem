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

    public List<string> GetScores(List<int> pinsDowned, List<List<int>> frames)
    {
        List<string> labels = new();
        int score = 0;
        int index = 0;

        for (var i = 0; i < frames.Count; i++)
        {
            int frameSum = frames[i].Sum();
            int frameLen = frames[i].Count;
            index += frameLen;

            score = StrategyVisitor(score, labels,
                (frameLen == 1 && frameSum == 10, new StrikeFrameScoreStrategy(pinsDowned, index)),
                (frameLen == 1 && frameSum < 10, new OpenFrameScoreStrategy(frameSum)),
                (frameLen == 2 && frameSum >= 10, new SpareFrameScoreStrategy(pinsDowned, index)),
                (frameLen == 2 && frameSum < 10, new ClosedFrameScoreStrategy(frameSum)),
                (i == frames.Count - 1 && frameLen < 4 && frameSum <= 30, new LastFrameScoreStrategy(frameSum)));
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