namespace FishingAlgoTest.Constants;

public static class GameConstants
{
    public const int SmallFishingPoleCost = 5;
    public const int MediumFishingPoleCost = 10;
    public const int BigFishingPoleCost = 15;

    public const int RedBaitCost = 1;
    public const int BlueBaitCost = 2;
    public const int GreenBaitCost = 3;

    public const int BestBaitWeight = 2;
    public const int OtherBaitWeight = 1;

    public const int SmallFishMinValue = 1;
    public const int SmallFishMaxValue = 5;
    public const int MediumFishMinValue = 5;
    public const int MediumFishMaxValue = 10;
    public const int BigFishMinValue = 10;
    public const int BigFishMaxValue = 15;

    public const int SmallFishMinCount = 3;
    public const int SmallFishMaxCount = 12;
    public const int MediumFishMinCount = 2;
    public const int MediumFishMaxCount = 8;
    public const int BigFishMinCount = 1;
    public const int BigFishMaxCount = 6;

    public const int RedFishMinPercentage = 20;
    public const int RedFishMaxPercentage = 50;
    public const int BlueFishMinPercentage = 25;
    public const int BlueFishMaxPercentage = 60;

    public static readonly int MinCastingDelayMilliseconds = TimeSpan.FromSeconds(2).Milliseconds;
    public static readonly int MaxCastingDelayMilliseconds = TimeSpan.FromSeconds(3).Milliseconds;

    public static readonly int MinJudgingDelayMilliseconds = TimeSpan.FromSeconds(1).Milliseconds;
    public static readonly int MaxJudgingDelayMilliseconds = TimeSpan.FromSeconds(2).Milliseconds;
}