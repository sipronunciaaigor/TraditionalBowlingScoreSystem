using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TraditionalBowlingServices;
using Xunit;

namespace TraditionalBowlingServicesTests
{
    public class ScoreServiceTest
    {
        private readonly ScoreService _scoreService;
        private readonly Fixture _fixture;
        private readonly List<int> _strike = new() { 10 };

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

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetScores_ShouldUseStrikeFrameScoreStrategy_OnlyUnknownResults(int totFrames)
        {
            // Arrange
            List<int> pinsDowned = new() { 10 };
            List<List<int>> frames = new(totFrames);
            for (int i = 0; i < totFrames; i++)
            {
                frames.Add(_strike);
            }

            // Act
            var results = _scoreService.GetScores(pinsDowned, frames);

            //Assert
            results.Should().HaveCount(totFrames);
            foreach (var result in results)
            {
                result.Should().Be("*");
            }
        }

        [Fact]
        public void GetScores_ShouldUseStrikeFrameScoreStrategy_UnknownResults()
        {
            // Arrange
            int totStrikes = 9;
            List<int> pinsDowned = new(totStrikes);
            for (int i = 0; i < totStrikes; i++)
            {
                pinsDowned.Add(10);
            }

            List<List<int>> frames = new(totStrikes);
            for (int i = 0; i < totStrikes; i++)
            {
                frames.Add(_strike);
            }

            // Act
            var results = _scoreService.GetScores(pinsDowned, frames);

            //Assert
            results.Should().HaveCount(totStrikes);
            for (int i = 1; i <= results.Count; i++)
            {
                if (i < totStrikes - 1)
                {
                    results[i - 1].Should().Be((i * 30).ToString());
                }
                else
                {
                    results[i - 1].Should().Be("*");
                }
            }
        }

        [Fact]
        public void GetScores_ShouldDeterminePerfectGame()
        {
            // Arrange
            int totStrikes = 12;
            int totFrames = 10;

            List<int> pinsDowned = new(totStrikes);
            for (int i = 0; i < totStrikes; i++)
            {
                pinsDowned.Add(10);
            }

            List<List<int>> frames = new(totFrames);
            for (int i = 0; i < totFrames; i++)
            {
                frames.Add(_strike);
            }

            // Act
            var results = _scoreService.GetScores(pinsDowned, frames);

            //Assert
            results.Should().HaveCount(totFrames);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].Should().Be(((i + 1) * 30).ToString());
            }
        }

        [Fact]
        public void GetScores_ShouldDetermineGutterBallGame()
        {
            // Arrange
            int totShots = 20;
            int totFrames = 10;
            List<int> gutterFrame = new(2) { 0, 0 };

            List<int> pinsDowned = new(totShots);
            for (int i = 0; i < totShots; i++)
            {
                pinsDowned.Add(0);
            }

            List<List<int>> frames = new(totFrames);
            for (int i = 0; i < totFrames; i++)
            {
                frames.Add(gutterFrame);
            }

            // Act
            var results = _scoreService.GetScores(pinsDowned, frames);

            //Assert
            results.Should().HaveCount(totFrames);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].Should().Be(0.ToString());
            }
        }

        [Theory, AutoData]
        public void GetScores_ShouldReturnGameOfSpares([Range(1, 9)] int firstShot)
        {
            // Arrange
            int secondShot = 10 - firstShot;
            int totShots = 21;
            int totFrames = 10;
            List<int> spareFrame = new(2) { firstShot, secondShot };

            List<int> pinsDowned = new(totShots);
            for (int i = 0; i < totShots; i++)
            {
                pinsDowned.Add(i % 2 == 0 ? firstShot : secondShot);
            }

            List<List<int>> frames = new(totFrames);
            for (int i = 0; i < totFrames - 1; i++)
            {
                frames.Add(spareFrame);
            }
            frames.Add(new(3) { firstShot, secondShot, firstShot });


            // Act
            var results = _scoreService.GetScores(pinsDowned, frames);

            //Assert
            results.Should().HaveCount(totFrames);
            for (int i = 0; i < results.Count; i++)
            {
                results[i].Should().Be(((i + 1) * (firstShot + secondShot + firstShot)).ToString());
            }
        }

        [Fact]
        public void GetScores_ShouldReturnComplexResult_Example2()
        {
            // Arrange
            List<int> pinsDowned = new(12);
            List<List<int>> frames = new(6);
            for (int i = 0; i < 12; i++)
            {
                pinsDowned.Add(1);
                if (i % 2 == 0)
                {
                    frames.Add(new() { 1, 1 });
                }
            }

            List<string> expectedResult = new() { "2", "4", "6", "8", "10", "12" };

            // Act
            var results = _scoreService.GetScores(pinsDowned, frames);

            //Assert
            results.Should().HaveCount(frames.Count);
            results.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void GetScores_ShouldReturnComplexResult_Example3()
        {
            // Arrange
            List<int> pinsDowned = new() { 1, 1, 1, 1, 9, 1, 2, 8, 9, 1, 10, 10 };

            List<List<int>> frames = new(7);
            frames.Add(new() { 1, 1 });
            frames.Add(new() { 1, 1 });
            frames.Add(new() { 9, 1 });
            frames.Add(new() { 2, 8 });
            frames.Add(new() { 9, 1 });
            frames.Add(new() { 10 });
            frames.Add(new() { 10 });

            List<string> expectedResult = new() { "2", "4", "16", "35", "55", "*", "*" };

            // Act
            var results = _scoreService.GetScores(pinsDowned, frames);

            //Assert
            results.Should().HaveCount(frames.Count);
            results.Should().BeEquivalentTo(expectedResult);
        }
    }
}