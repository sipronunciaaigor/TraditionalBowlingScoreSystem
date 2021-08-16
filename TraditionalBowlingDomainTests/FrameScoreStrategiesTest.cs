using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using TraditionalBowlingDomain;
using Xunit;

namespace TraditionalBowlingServicesTests
{
    public class FrameScoreStrategiesTest
    {
        private readonly Fixture _fixture;

        public FrameScoreStrategiesTest()
        {
            _fixture = new();
            _fixture.Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        public void ClosedFrameScoreStrategyLabel_ShouldBeKnown()
        {
            // Arrange
            int frameSum = _fixture.Create<int>();
            ClosedFrameScoreStrategy strategy = new(frameSum);

            // Act
            var result = strategy.GetScoreLabel();

            // Assert
            result.Score.Should().Be(frameSum);
            result.UnknownLabel.Should().BeFalse();
        }

        [Fact]
        public void LastFrameScoreStrategyLabel_ShouldBeKnown()
        {
            // Arrange
            int frameSum = _fixture.Create<int>();
            LastFrameScoreStrategy strategy = new(frameSum);

            // Act
            var result = strategy.GetScoreLabel();

            // Assert
            result.Score.Should().Be(frameSum);
            result.UnknownLabel.Should().BeFalse();
        }

        [Fact]
        public void OpenFrameScoreStrategyLabel_ShouldBeUnknown()
        {
            // Arrange
            int frameSum = _fixture.Create<int>();
            OpenFrameScoreStrategy strategy = new(frameSum);

            // Act
            var result = strategy.GetScoreLabel();

            // Assert
            result.Score.Should().Be(frameSum);
            result.UnknownLabel.Should().BeTrue();
        }

        [Theory]
        [InlineData(new[] { 9, 1 }, true)]
        [InlineData(new[] { 1, 9, 5 }, false)]
        public void SpareFrameScoreStrategyLabel_ShouldChange(int[] pinsDowned, bool labelStatus)
        {
            // Arrange
            List<int> pinsDownedList = pinsDowned.ToList();
            SpareFrameScoreStrategy strategy = new(pinsDownedList, 2);

            // Act
            var result = strategy.GetScoreLabel();

            // Assert
            result.Score.Should().Be(pinsDownedList.Take(3).Sum());
            result.UnknownLabel.Should().Be(labelStatus);
        }

        [Theory]
        [InlineData(new[] { 10 }, true)]
        [InlineData(new[] { 10, 10 }, true)]
        [InlineData(new[] { 10, 5, 5 }, false)]
        [InlineData(new[] { 10, 10, 5, 5 }, false)]
        public void StrikeFrameScoreStrategyLabel_ShouldChange(int[] pinsDowned, bool labelStatus)
        {
            // Arrange
            List<int> pinsDownedList = pinsDowned.ToList();
            StrikeFrameScoreStrategy strategy = new(pinsDownedList, 1);

            // Act
            var result = strategy.GetScoreLabel();

            // Assert
            result.Score.Should().Be(pinsDownedList.Take(3).Sum());
            result.UnknownLabel.Should().Be(labelStatus);
        }
    }
}