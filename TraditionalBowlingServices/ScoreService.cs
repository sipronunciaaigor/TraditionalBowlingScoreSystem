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

            score = ExecuteStrategy(
                (frameLen == 1 && frameSum == 10, () => StrikeStrategy(pinsDowned, index, score, labels)),
                (frameLen == 1 && frameSum < 10, () => OpenStrategy(score, frameSum, labels)),
                (frameLen == 2 && frameSum >= 10, () => SpareStrategy(pinsDowned, index, score, labels)),
                (frameLen == 2 && frameSum < 10, () => ClosedStrategy(score, frameSum, labels)),
                (i == frames.Count - 1 && frameLen < 4 && frameSum <= 30, () => LastStrategy(score, frameSum, labels))
            );
        }

        return labels;
    }

    public static int ExecuteStrategy(params (bool condition, Func<int> strategy)[] strategies)
    {
        var t = strategies.First(tuple => tuple.condition);
        var r = t.strategy.Invoke();
        return r;
    }

    public static int LastStrategy(int score, int frameSum, List<string> labels)
    {
        score += frameSum;
        labels.Add(score.ToString());
        return score;
    }

    public static int ClosedStrategy(int score, int frameSum, List<string> labels)
    {
        score += frameSum;
        labels.Add(score.ToString());
        return score;
    }

    public static int OpenStrategy(int score, int frameSum, List<string> labels)
    {
        score += frameSum;
        labels.Add("*");
        return score;
    }

    public static int SpareStrategy(List<int> pinsDowned, int index, int score, List<string> labels)
    {
        var nextOne = pinsDowned.Skip(index).Take(1).ToList();
        score += 10 + nextOne.Sum();
        labels.Add(nextOne.Count < 1 ? "*" : score.ToString());
        return score;
    }

    public static int StrikeStrategy(List<int> pinsDowned, int index, int score, List<string> labels)
    {
        var nextTwo = pinsDowned.Skip(index).Take(2).ToList();
        score += 10 + nextTwo.Sum();
        labels.Add(nextTwo.Count < 2 ? "*" : score.ToString());
        return score;
    }
}