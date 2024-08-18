namespace FishingAlgoTest.Enums;

/// <summary>
/// Represents the result of a player's performance.
/// Currently, the game only supports win, lose, and tie.
/// Player can win if they have more gold than 100, lose if they have less than or equal to
/// 100, and tie if they skip the day.
/// </summary>
public enum PerformanceResult
{
    Win,
    Lose,
    Tie
}