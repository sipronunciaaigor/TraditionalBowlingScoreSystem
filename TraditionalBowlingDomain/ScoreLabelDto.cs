namespace TraditionalBowlingDomain;

public record ScoreLabelDto
{
    public int Score { get; set; }
    public bool UnknownLabel { get; set; }
}