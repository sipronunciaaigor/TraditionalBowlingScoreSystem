using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TraditionalBowlingDomain;

namespace TraditionalBowlingServices
{
    public interface IGameService
    {
        GameProgressResponseDto GetScores(List<int> pinsDowned);
    }

    public class GameService : IGameService
    {
        private readonly IFrameService _frameService;
        private readonly IScoreService _scoreService;
        private readonly ILogger<GameService> _logger;

        public GameService(IFrameService frameService, IScoreService scoreService, ILogger<GameService> logger)
        {
            _frameService = frameService;
            _scoreService = scoreService;
            _logger = logger;
        }

        public GameProgressResponseDto GetScores(List<int> pinsDowned)
        {
            var frames = _frameService.GetFrames(pinsDowned);
            var labels = _scoreService.GetScores(pinsDowned, frames);
            return new GameProgressResponseDto { FrameProgressScores = labels };
        }
    }
}