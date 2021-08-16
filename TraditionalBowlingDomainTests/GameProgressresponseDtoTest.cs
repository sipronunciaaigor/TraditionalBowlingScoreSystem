using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using System.Linq;
using TraditionalBowlingDomain;
using Xunit;

namespace TraditionalBowlingServicesTests
{
    public class GameProgressResponseDtoTest
    {
        private readonly Fixture _fixture;

        public GameProgressResponseDtoTest()
        {
            _fixture = new();
            _fixture.Customize(new AutoNSubstituteCustomization());
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(9, false)]
        [InlineData(10, true)]
        public void GameCompleted_ShouldChange(int frames, bool completed)
        {
            // Arrange and act
            var gameProgressResponseDto = _fixture
                .Build<GameProgressResponseDto>()
                .With(x => x.FrameProgressScores, _fixture.CreateMany<string>(frames).ToList())
                .Create();

            // Assert
            gameProgressResponseDto.GameCompleted.Should().Be(completed);
        }

        [Fact]
        public void GameCompleted_ShouldBeFalse_When10thFrameOpen()
        {
            // Arrange and act
            var gameProgressResponseDto = _fixture
                .Build<GameProgressResponseDto>()
                .With(x => x.FrameProgressScores, _fixture.CreateMany<string>(9).ToList())
                .Create();

            gameProgressResponseDto.FrameProgressScores.Add("*");
            // Assert
            gameProgressResponseDto.GameCompleted.Should().BeFalse();
        }
    }
}