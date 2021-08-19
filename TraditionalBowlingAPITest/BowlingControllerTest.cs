using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using TraditionalBowlingDomain;
using TraditionalBowlingScoreSystem.Controllers;
using TraditionalBowlingServices;
using Xunit;

namespace TraditionalBowlingServicesTests
{
    public class BowlingControllerTest
    {
        private readonly Fixture _fixture;
        private readonly IGameService _gameService;
        private readonly BowlingController _bowlingController;

        public BowlingControllerTest()
        {
            _fixture = new();
            _fixture.Customize(new AutoNSubstituteCustomization());
            _fixture.Freeze<ILogger<BowlingController>>();
            _gameService = _fixture.Freeze<IGameService>();
            _bowlingController = _fixture.Create<BowlingController>();
        }

        [Fact]
        public void BowlingController_ShouldReturnOk()
        {
            // Arrange
            ScoreProgressRequestDto requestDto = _fixture.Create<ScoreProgressRequestDto>();
            GameProgressResponseDto gameProgressResponseDto= _fixture.Create<GameProgressResponseDto>();
            _gameService.GetScores(requestDto.PinsDowned).Returns(gameProgressResponseDto);

            // Act
            var result = _bowlingController.Scores(requestDto);
                
            // Assert
            result.Should().Be(gameProgressResponseDto);
        }
    }
}