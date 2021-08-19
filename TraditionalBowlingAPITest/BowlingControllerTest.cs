using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
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

        private readonly ScoreProgressRequestDto _requestDto;

        public BowlingControllerTest()
        {
            _fixture = new();
            _fixture.Customize(new AutoNSubstituteCustomization());
            _fixture.Freeze<ILogger<BowlingController>>();

            var myLogger = Substitute.For<ILogger<BowlingController>>();
            _gameService = _fixture.Freeze<IGameService>();

            _requestDto = _fixture.Create<ScoreProgressRequestDto>();

            _bowlingController = new BowlingController(
                _gameService,
                myLogger
                );

        }

        [Fact]
        public void BowlingController_ShouldReturnOk()
        {
            // Arrange
            GameProgressResponseDto gameProgressResponseDto = _fixture.Create<GameProgressResponseDto>();
            _gameService.GetScores(_requestDto.PinsDowned).Returns(gameProgressResponseDto);

            // Act
            var result = (OkObjectResult)_bowlingController.Scores(_requestDto);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public void BowlingController_ShouldReturn400()
        {
            // Arrange
            _gameService.GetScores(_requestDto.PinsDowned).Throws<ArgumentOutOfRangeException>();

            // Act
            var result = (BadRequestObjectResult)_bowlingController.Scores(_requestDto);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public void BowlingController_ShouldReturn500()
        {
            // Arrange
            _gameService.GetScores(_requestDto.PinsDowned).Throws<Exception>();

            // Act
            var result = (StatusCodeResult)_bowlingController.Scores(_requestDto);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}