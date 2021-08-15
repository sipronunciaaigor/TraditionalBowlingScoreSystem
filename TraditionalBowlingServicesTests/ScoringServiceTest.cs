using FluentAssertions;
using TraditionalBowlingServices;
using Xunit;

namespace TraditionalBowlingServicesTests;

public class ScoringServiceTest
{
    public ScoringServiceTest()
    {
    }

    [Theory]
    [InlineData(new int[] { }, "")]
    [InlineData(new int[] { 0 }, "[0]")]
    [InlineData(new int[] { 1, 1 }, "[1,1]")]
    [InlineData(new int[] { 1, 1, 1 }, "[1,1][1]")]
    [InlineData(new int[] { 10, 1, 1 }, "[10][1,1]")]
    [InlineData(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "[0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0][0]")]
    [InlineData(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "[0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0]")] // gutter-ball
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, "[10][10][10][10][10][10][10][10][10][10]")] // start of last frame
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, "[10][10][10][10][10][10][10][10][10][10,10]")] // second shot of last frame
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, "[10][10][10][10][10][10][10][10][10][10,10,10]")] // perfect match
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 9, 1, 10 }, "[10][10][10][10][10][10][10][10][10][9,1,10]")]
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 1, 9 }, "[10][10][10][10][10][10][10][10][10][10,1,9]")]
    public void GetFrames_ShouldReturnProperFrames(int[] shots, string expectedFrames)
    {
        // Arrange
        string result = string.Empty; // can use string builder

        // Act
        var frames = ScoringService.Frames.GetFrames(shots.ToList());
        for (int i = 0; i < frames.Count; i++)
        {
            result = string.Concat(result, "[" + string.Join(',', frames[i]) + "]");
        }

        // Assert
        result.Should().Be(expectedFrames);
    }

    /// <summary>
    /// Covers the following shape: { 10, 10, 10 }, "[10][10][10]")] from 0 to 9 shots
    /// </summary>
    [Fact]
    public void GetFrames_ShouldReturnProperFramesSeriesOf10s_ExcludesLastFrame()
    {
        // Arrange
        int shots = 9;
        for (int i = 0; i < shots; i++)
        {
            const int pinDowns = 10;
            string expectedFrames = string.Empty; // can use string builder
            string result = string.Empty; // can use string builder
            List<int> allShots = new(shots);
            for (int j = 0; j < shots; j++)
            {
                allShots.Add(pinDowns);
                expectedFrames = string.Concat(expectedFrames, "[" + pinDowns + "]");
            }

            // Act
            var frames = ScoringService.Frames.GetFrames(allShots);
            for (int j = 0; j < frames.Count; j++)
            {
                result = string.Concat(result, "[" + string.Join(',', frames[j]) + "]");
            }

            // Assert
            result.Should().Be(expectedFrames);
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(11)]
    public void GetFrames_ShouldThrow_ScoreBetween0And10(int shot)
    {
        // Arrange
        int[] shotArray = new int[1] { shot };

        // Act
        Action getFrames = () => ScoringService.Frames.GetFrames(shotArray.ToList());

        // Assert
        getFrames.Should().Throw<ArgumentOutOfRangeException>("score").WithMessage($"Score {shot} not valid. Must be between 0 and 10 (Parameter 'score')");
    }

    [Theory]
    [InlineData(new int[] { 5, 6 })]
    [InlineData(new int[] { 9, 9 })]
    public void GetFrames_ShouldThrow_FrameSumBiggerThan10(int[] shots)
    {
        // Arrange

        // Act
        Action getFrames = () => ScoringService.Frames.GetFrames(shots.ToList());

        // Assert
        getFrames.Should().Throw<ArgumentOutOfRangeException>("sum").WithMessage($"Frame 1 not valid. Must be between 0 and 10 (Parameter 'sum')");
    }

    [Fact]
    public void GetFrames_ShouldThrow_TooManyShotsOnLastFrame()
    {
        // Arrange
        const int baseShots = 9;
        const int lastFrameShots = 4;
        const int totShots = baseShots + lastFrameShots;
        List<int> allShots = new(totShots);
        for (int i = 0; i < totShots; i++)
        {
            allShots.Add(10);
        }
        // Act
        Action getFrames = () => ScoringService.Frames.GetFrames(allShots.ToList());

        // Assert
        getFrames.Should().Throw<ArgumentOutOfRangeException>("lastShots").WithMessage($"Last frame not valid. Cannot contain more than 3 shots. It contains {lastFrameShots} (Parameter 'lastShots')");
    }

    [Fact]
    public void GetFrames_ShouldThrow_WrongSumOnFirstShotsOfLastFrame()
    {
        // Arrange
        const int baseShots = 9;
        const int lastTwoShotsValue = 8;
        List<int> allShots = new(baseShots);
        for (int i = 0; i < baseShots; i++)
        {
            allShots.Add(10);
        }
        allShots.Add(lastTwoShotsValue);
        allShots.Add(lastTwoShotsValue);

        // Act
        Action getFrames = () => ScoringService.Frames.GetFrames(allShots.ToList());

        // Assert
        getFrames.Should().Throw<ArgumentOutOfRangeException>("sum").WithMessage("Invalid shots in the last frame. (Parameter 'sum')");
    }

    [Fact]
    public void GetFrames_ShouldThrow_CannotThrowIfFirstTwoNotStrikeOrSpare()
    {
        // Arrange
        const int baseShots = 9;
        const int lastShotsValue = 3;
        List<int> allShots = new(baseShots);
        for (int i = 0; i < baseShots; i++)
        {
            allShots.Add(10);
        }

        allShots.Add(lastShotsValue);
        allShots.Add(lastShotsValue);
        allShots.Add(lastShotsValue);

        // Act
        Action getFrames = () => ScoringService.Frames.GetFrames(allShots.ToList());

        // Assert
        getFrames.Should().Throw<ArgumentOutOfRangeException>("lastShots").WithMessage($"Last frame not valid. Not allowed to throw the last ball (Parameter 'lastShots')");
    }
}