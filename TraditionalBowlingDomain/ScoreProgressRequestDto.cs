using System.Collections.Generic;

namespace TraditionalBowlingDomain
{
    public record ScoreProgressRequestDto
    {
        public List<int> PinsDowned { get; set; } = new(21);
    }
}