
namespace TraditionalBowlingServices;

public class ScoringService
{
    public class Frames
    {
        public static List<List<int>> GetFrames(List<int> input)
        {
            List<List<int>> frames = new();
            List<int> shots = new();

            for (int i = 0; i < input.Count; i++)
            {
                var score = input[i];
                if (score < 0 || score > 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(score), $"Score {score} not valid. Must be between 0 and 10");
                }

                shots.Add(score);

                int sum = shots.Sum();
                if (sum > 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(sum), $"Frame {i} not valid. Must be between 0 and 10");
                }

                // add frame when 2 shots, spare or strike
                if (sum == 10 || shots.Count == 2)
                {
                    frames.Add(shots.ToList());
                    shots.Clear();
                }

                // take everything else
                if (frames.Count == 9)
                {
                    var leftovers = input.Skip(i + 1).ToList();
                    if (leftovers.Count > 3)
                    {
                        throw new ArgumentOutOfRangeException(nameof(leftovers), $"Last frame not valid. Cannot contain more than 3 shots. It contains {leftovers.Count}");
                    }

                    frames.Add(leftovers);
                    shots.Clear();
                    break;
                }
            }

            // take shots when game not ended
            if (shots.Count > 0)
            {
                frames.Add(shots);
            }
            return frames;
        }
    }

    public class Scores
    {
        public static List<string> GetScores(List<int> input, List<List<int>> frames)
        {
            List<string> labels = new();
            var score = 0;
            var index = 0;

            for (var i = 0; i < frames.Count; i++)
            {
                var frameSum = frames[i].Sum();
                var frameLen = frames[i].Count;
                index += frameLen;

                if (frameLen == 1 && frameSum == 10)
                {
                    var nextTwo = input.Skip(index).Take(index + 2).ToList();
                    score += 10 + nextTwo.Sum();
                    labels.Add(nextTwo.Count < 2 ? "*" : score.ToString());
                    continue;
                }
                if (frameLen == 1 && frameSum < 10)
                {
                    score += frameSum;
                    labels.Add("*");
                    continue;
                }
                if (frameLen == 2 && frameSum == 10)
                {
                    var nextOne = input.Skip(index).Take(index + 1).ToList();
                    score += 10 + nextOne.Sum();
                    labels.Add(nextOne.Count < 1 ? "*" : score.ToString());
                    continue;
                }
                if (frameLen == 2 && frameSum < 10)
                {
                    score += frames[i].Sum();
                    labels.Add(score.ToString());
                    continue;
                }
                if (i == frames.Count - 1 && frameLen < 4 && frameSum <= 30)
                {
                    score += frames[i].Sum();
                    labels.Add(score.ToString());
                    continue;
                }
            }
            return labels;
        }
    }
}
