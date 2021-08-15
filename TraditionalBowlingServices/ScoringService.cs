
using TraditionalBowlingDomain;

namespace TraditionalBowlingServices;

public class ScoringService
{
    public class Frames
    {
        public static List<List<int>> GetFrames(List<int> pinsDowned)
        {
            List<List<int>> frames = new();
            List<int> shots = new();

            for (int i = 0; i < pinsDowned.Count && frames.Count < 10; i++)
            {
                var score = pinsDowned[i];
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

                if (frames.Count == 9)
                {
                    var lastShots = pinsDowned.Skip(i + 1).ToList();
                    var lastShotsCount = lastShots.Count;

                    if (lastShotsCount > 0)
                    {
                        ValidateLastFrame(lastShots, lastShotsCount);
                        frames.Add(lastShots);
                    }
                    shots.Clear();
                }
            }

            // take shots when game not ended
            if (shots.Count > 0)
            {
                frames.Add(shots);
            }
            return frames;
        }

        private static void ValidateLastFrame(List<int> lastShots, int lastShotsCount)
        {
            if (lastShotsCount > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(lastShots), $"Last frame not valid. Cannot contain more than 3 shots. It contains {lastShotsCount}");
            }

            for (int i = 0; i < lastShots.Count - 1; i++)
            {
                if (lastShots[i] != 10 && lastShots[i] + lastShots[i + 1] > 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(lastShots), $"Last frame not valid on its shots {i + 1} and {i + 2}. Their sum is {lastShots[i] + lastShots[i + 1]}.");
                }
            }

            var firstTwoLastShotsSum = lastShots.Take(2).Sum();
            if (firstTwoLastShotsSum > 10 && lastShots[0] < 10 && firstTwoLastShotsSum != 20)
            {
                throw new ArgumentOutOfRangeException(nameof(firstTwoLastShotsSum), $"First two shots of last frame not valid. Their sum is {firstTwoLastShotsSum} and exceeds 10");
            }
            else if (firstTwoLastShotsSum < 10 && lastShotsCount == 3)
            {
                throw new ArgumentOutOfRangeException(nameof(lastShots), $"Last frame not valid. Not allowed to throw the last ball");
            }
        }
    }

    public class Scores
    {
        public static GameProgressResponseDto GetScores(List<int> pinsDowned, List<List<int>> frames)
        {
            List<string> labels = new();
            var score = 0;
            var index = 0;

            for (var i = 0; i < frames.Count; i++)
            {
                var frameSum = frames[i].Sum();
                var frameLen = frames[i].Count;
                index += frameLen;

                // strike frame strategy
                if (frameLen == 1 && frameSum == 10)
                {
                    var nextTwo = pinsDowned.Skip(index).Take(2).ToList();
                    score += 10 + nextTwo.Sum();
                    labels.Add(nextTwo.Count < 2 ? "*" : score.ToString());
                    continue;
                }
                // open frame strategy
                if (frameLen == 1 && frameSum < 10)
                {
                    score += frameSum;
                    labels.Add("*");
                    continue;
                }
                // spare frame strategy
                if (frameLen == 2 && frameSum >= 10)
                {
                    var nextOne = pinsDowned.Skip(index).Take(1).ToList();
                    score += 10 + nextOne.Sum();
                    labels.Add(nextOne.Count < 1 ? "*" : score.ToString());
                    continue;
                }
                // closed frame strategy
                if (frameLen == 2 && frameSum < 10)
                {
                    score += frameSum;
                    labels.Add(score.ToString());
                    continue;
                }
                // last frame strategy
                if (i == frames.Count - 1 && frameLen < 4 && frameSum <= 30)
                {
                    score += frameSum;
                    labels.Add(score.ToString());
                    continue;
                }


                // error strategy
                labels.Add("err");
            }
            return new GameProgressResponseDto() { FrameProgressScores = labels };
        }
    }
}
