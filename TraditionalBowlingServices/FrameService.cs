using Microsoft.Extensions.Logging;

namespace TraditionalBowlingServices;

public interface IFrameService
{
    List<List<int>> GetFrames(List<int> PinsDowned);
}

public class FrameService : IFrameService
{
    private readonly ILogger<FrameService> _logger;

    public FrameService(ILogger<FrameService> logger)
    {
        _logger = logger;
    }
    //
    // public List<Frame> GetFrames(List<int> pinsDowned)
    // {
    //     List<Frame> frames = new();
    //     List<int> shots = new();
    //
    //     for (int i = 0; i < pinsDowned.Count && frames.Count < 10; i++)
    //     {
    //         Frame frame = new(i);
    //         frame.AddShot(pinsDowned[i]);
    //
    //         // add frame when 2 shots, spare or strike
    //         if (frame.Sum == 10 || frame.Count == 2)
    //         {
    //             frames.Add(shots.ToList());
    //             shots.Clear();
    //         }
    //
    //         if (frames.Count == 9)
    //         {
    //             var lastShots = pinsDowned.Skip(i + 1).ToList();
    //             var lastShotsCount = lastShots.Count;
    //
    //             if (frame.IlastShotsCount > 0)
    //             {
    //                 ValidateLastFrame(lastShots, lastShotsCount);
    //                 frames.Add(lastShots);
    //             }
    //
    //             shots.Clear();
    //         }
    //     }
    //
    //     // take shots when game not ended
    //     if (shots.Count > 0)
    //     {
    //         frames.Add(shots);
    //     }
    //
    //     return frames;
    // }

    public List<List<int>> GetFrames(List<int> pinsDowned)
    {
        List<List<int>> frames = new();
        List<int> shots = new();

        for (int i = 0; i < pinsDowned.Count && frames.Count < 10; i++)
        {
            var score = pinsDowned[i];
            if (score < 0 || score > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(score),
                    $"Score {score} not valid. Must be between 0 and 10");
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
            throw new ArgumentOutOfRangeException(nameof(lastShots),
                $"Last frame not valid. Cannot contain more than 3 shots. It contains {lastShotsCount}");
        }

        var firstTwoLastShotsSum = lastShots.Take(2).Sum();
        if (firstTwoLastShotsSum < 10 && lastShotsCount == 3)
        {
            throw new ArgumentOutOfRangeException(nameof(lastShots),
                $"Last frame not valid. Not allowed to throw the last ball");
        }

        List<int> shots = new(3);
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