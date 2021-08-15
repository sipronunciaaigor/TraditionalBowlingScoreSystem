using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using TraditionalBowlingServices;
using Xunit;

namespace TraditionalBowlingServicesTests;

public class ScoreServiceTest
{
    private readonly ScoreService _scoreService;
    private readonly Fixture _fixture;

    public ScoreServiceTest()
    {
        _fixture = new();
        _fixture.Customize(new AutoNSubstituteCustomization());
        _fixture.Freeze<ILogger<ScoreService>>();
        _scoreService = _fixture.Create<ScoreService>();
    }

    [Fact]
    public void GetScores_ShouldReturnEmptyList_WhenNoFrames()
    {
        // Arrange
        List<int> pinsDowned = _fixture.CreateMany<int>(0).ToList();
        List<List<int>> frames = _fixture.CreateMany<List<int>>(0).ToList();

        // Act
        var result = _scoreService.GetScores(pinsDowned, frames);

        //Assert
        result.Should().BeEmpty();
    }

    // [Theory]
    // [InlineData(1)]
    // [InlineData(2)]
    // public void GetScores_ShouldUseStrikeFrameScoreStrategy(int totFrames)
    // {
    //     // Arrange
    //     List<int> strike = _fixture.Build<int>().With(x => x, 10).CreateMany(1).ToList();
    //     List<int> pinsDowned = _fixture.Build<int>().With(x => x, 10).CreateMany(totFrames).ToList();
    //     List<List<int>> frames = _fixture.Build<List<int>>().With(x => x, strike).CreateMany(totFrames).ToList();
    //
    //     // Act
    //     var results = _scoreService.GetScores(pinsDowned, frames);
    //
    //     //Assert
    //     results.Count.Should().Be(totFrames);
    //     foreach (var result in results)
    //     {
    //         result.Should().Be("*");
    //     }
    // }
}