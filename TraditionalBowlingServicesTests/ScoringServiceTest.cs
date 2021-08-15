using TraditionalBowlingServices;
using Xunit;
using FluentAssertions;

namespace TraditionalBowlingServicesTests;

public class ScoringServiceTest
{
    public ScoringServiceTest()
    { }

    [Theory]
    [InlineData(new int[] { }, "")]
    [InlineData(new int[] { 0 }, "[0]")]
    [InlineData(new int[] { 1, 1 }, "[1,1]")]
    [InlineData(new int[] { 1, 1, 1 }, "[1,1][1]")]
    [InlineData(new int[] { 10, 1, 1 }, "[10][1,1]")]
    [InlineData(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "[0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0][0]")]
    [InlineData(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "[0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0][0,0]")] // gutter-ball
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 9, 1, 10 }, "[10][10][10][10][10][10][10][10][10][9,1,10]")]
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, "[10][10][10][10][10][10][10][10][10][10]")] // start of last frame
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, "[10][10][10][10][10][10][10][10][10][10,10]")] // second shot of last frame
    [InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, "[10][10][10][10][10][10][10][10][10][10,10,10]")] // perfect match
    public void GetFrames_ShouldReturnProperFrames(int[] shots, string expectedFrames)
    {
        // Arrange
        string result = string.Empty;

        // Act
        var frames = ScoringService.Frames.GetFrames(shots.ToList());

        // Assert
        for (int i = 0; i < frames.Count; i++)
        {
            result = string.Concat(result, "[" + string.Join(',', frames[i]) + "]");
        }

        result.Should().Be(expectedFrames);
    }

    //[Theory]
    //[InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, "[10][10][10][10][10][10][10][10][10][10,10]")]
    //[InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 })]
    //[InlineData(new int[] { 9, 1, 1, 9, 9, 1, 9, 9, 9, 9, 9, 9, 9 })]
    //[InlineData(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 9, 0, 1 })]
    //[InlineData(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 })]
    //public void GetFrames_ShouldThrow(int[] shots, string expectedFrames)
    //{ }
}