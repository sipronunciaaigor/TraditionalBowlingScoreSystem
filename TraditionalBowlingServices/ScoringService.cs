
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

            var firstTwoLastShotsSum = lastShots.Take(2).Sum();
            if (firstTwoLastShotsSum < 10 && lastShotsCount == 3)
            {
                throw new ArgumentOutOfRangeException(nameof(lastShots), $"Last frame not valid. Not allowed to throw the last ball");
            }
            
            List<int> shots = new (3);
            for (int i = 0; i < lastShots.Count; i++)
            {
                var score = lastShots[i];
                shots.Add(score);
                var sum = shots.Sum();
                if (sum > 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(sum), $"Invalid shots in the last frame.");
                }
                else if (sum == 10)
                {
                    shots.Clear();
                }
            }
        }
    }

    public class Scores
    {
        public static GameProgressResponseDto GetScores(List<int> pinsDowned, List<List<int>> frames)
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
            return new GameProgressResponseDto() { FrameProgressScores = labels };
        }
        
        public static int ExecuteStrategy(params(bool condition, Func<int> strategy)[] strategies)
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
}
