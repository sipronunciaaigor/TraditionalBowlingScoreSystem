// namespace TraditionalBowlingDomain;
//
// public class Frame
// {
//     public int Index { get; }
//     public List<int> Shots { get; set; } = new();
//     public int Sum => Shots.Sum();
//     public int Count => Shots.Count;
//
//     public Frame(int index)
//     {
//         Index = index;
//     }
//
//     public void AddShot(int pinsDowned)
//     {
//         if (pinsDowned < 0 || pinsDowned > 10)
//         {
//             throw new ArgumentOutOfRangeException(nameof(pinsDowned), "Shot not valid. Must be between 0 and 10");
//         }
//
//         Shots.Add(pinsDowned);
//
//         if (Sum > 10)
//         {
//             throw new ArgumentOutOfRangeException(nameof(Sum), $"Frame {Index} not valid. Must be between 0 and 10");
//         }
//     }
//
//     public void Clear()
//     {
//         Shots.Clear();
//     }
//     
//     private bool IsValidaLastFrame(List<int> pinsDowned)
//     {
//         if (lastShotsCount > 0)
//         {
//             if (lastShotsCount > 3)
//             {
//                 throw new ArgumentOutOfRangeException(nameof(lastShots),
//                     $"Last frame not valid. Cannot contain more than 3 shots. It contains {lastShotsCount}");
//             }
//
//             var firstTwoLastShotsSum = lastShots.Take(2).Sum();
//             if (firstTwoLastShotsSum < 10 && lastShotsCount == 3)
//             {
//                 throw new ArgumentOutOfRangeException(nameof(lastShots),
//                     $"Last frame not valid. Not allowed to throw the last ball");
//             }
//
//             List<int> shots = new(3);
//             for (int i = 0; i < lastShots.Count; i++)
//             {
//                 var score = lastShots[i];
//                 shots.Add(score);
//                 var sum = shots.Sum();
//                 if (sum > 10)
//                 {
//                     throw new ArgumentOutOfRangeException(nameof(sum), $"Invalid shots in the last frame.");
//                 }
//                 else if (sum == 10)
//                 {
//                     shots.Clear();
//                 }
//             }
//         }
//     }
// }