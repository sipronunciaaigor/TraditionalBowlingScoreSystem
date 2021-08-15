
namespace TraditionalBowlingServices;


// 1. find frames
// 2. analyse each frame
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
                shots.Add(input[i]);
                int sum = shots.Sum();

                // take 2 shots
                if (sum == 10 || shots.Count == 2)
                {
                    frames.Add(shots.ToList());
                    shots.Clear();
                }
                // take 1 shot
                else if (sum > 10)
                {
                    frames.Add(shots.Take(shots.Count - 1).ToList());
                    shots = new(input[i]);
                }

                // on 9th take all that's left
                if (frames.Count == 9)
                {
                    frames.Add(input.Skip(i).ToList());
                    shots.Clear();
                    break;
                }
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
