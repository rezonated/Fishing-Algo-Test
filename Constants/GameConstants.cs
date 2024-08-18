namespace FishingAlgoTest.Constants;

/// <summary>
/// Constants for the game.
/// Can be used to configure game parameters and values.
/// </summary>
public static class GameConstants
{
    #region Fishing Pole Constants
    
    /// <summary>
    /// The cost of a small fishing pole.
    /// </summary>
    public const int SmallFishingPoleCost = 5;
    
    /// <summary>
    /// The cost of a medium fishing pole.
    /// </summary>
    public const int MediumFishingPoleCost = 10;
    
    /// <summary>
    /// The cost of a big fishing pole.
    /// </summary>
    public const int BigFishingPoleCost = 15;
    
    #endregion

    #region Bait Constants
    
    /// <summary>
    /// The cost of a red bait.
    /// </summary>
    public const int RedBaitCost = 1;
    
    /// <summary>
    /// The cost of a blue bait.
    /// </summary>
    public const int BlueBaitCost = 2;
    
    /// <summary>
    /// The cost of a green bait.
    /// </summary>
    public const int GreenBaitCost = 3;
    
    #endregion

    #region Bait Weight Constants
    
    /// <summary>
    /// The weight of the best bait to buy.
    /// Used for auto buying baits, determining the best bait to buy.
    /// </summary>
    public const int BestBaitWeight = 2; 
    
    /// <summary>
    /// The weight of the other bait to buy.
    /// Used for auto buying baits, determining the other bait to buy to balance the best bait a bit.
    /// </summary>
    public const int OtherBaitWeight = 1;
    
    #endregion

    #region Fish Value Constants
    
    /// <summary>
    /// The minimum value of a small fish to randomly generate.
    /// </summary>
    public const int SmallFishMinValue = 1;
    
    /// <summary>
    /// The maximum value of a small fish to randomly generate.
    /// </summary>
    public const int SmallFishMaxValue = 5;
    
    /// <summary>
    /// The minimum value of a medium fish to randomly generate.
    /// </summary>
    public const int MediumFishMinValue = 5;
    
    /// <summary>
    /// The maximum value of a medium fish to randomly generate.
    /// </summary>
    public const int MediumFishMaxValue = 10;
    
    /// <summary>
    /// The minimum value of a big fish to randomly generate.
    /// </summary>
    public const int BigFishMinValue = 10;
    
    /// <summary>
    /// The maximum value of a big fish to randomly generate.
    /// </summary>
    public const int BigFishMaxValue = 15;
    
    #endregion

    #region Fish Count Constants
    
    /// <summary>
    /// The minimum count of small fish to randomly generate.
    /// </summary>
    public const int SmallFishMinCount = 3;
    
    /// <summary>
    /// The maximum count of small fish to randomly generate.
    /// </summary>
    public const int SmallFishMaxCount = 12;
    
    /// <summary>
    /// The minimum count of medium fish to randomly generate.
    /// </summary>
    public const int MediumFishMinCount = 2;
    
    /// <summary>
    /// The maximum count of medium fish to randomly generate.
    /// </summary>
    public const int MediumFishMaxCount = 8;
    
    /// <summary>
    /// The minimum count of big fish to randomly generate.
    /// </summary>
    public const int BigFishMinCount = 1;
    
    /// <summary>
    /// The maximum count of big fish to randomly generate.
    /// </summary>
    public const int BigFishMaxCount = 6;
    
    #endregion

    #region Fish Percentage Constants
    
    /// <summary>
    /// The minimum percentage of red fish to randomly generate.
    /// </summary>
    public const int RedFishMinPercentage = 20;
    
    /// <summary>
    /// The maximum percentage of red fish to randomly generate.
    /// </summary>
    public const int RedFishMaxPercentage = 50;
    
    /// <summary>
    /// The minimum percentage of blue fish to randomly generate.
    /// </summary>
    public const int BlueFishMinPercentage = 25;
    
    /// <summary>
    /// The maximum percentage of blue fish to randomly generate.
    /// </summary>
    public const int BlueFishMaxPercentage = 60;
    
    // There are no green percentages, as there will be calculated based on the other percentages. (e.g., 100 - redPercentage - bluePercentage)
    
    #endregion

    #region Delay Constants
    
    /// <summary>
    /// The minimum delay in milliseconds for casting a bait in milliseconds to randomly generate.
    /// </summary>
    public const int MinCastingDelayMilliseconds = 2000;
    
    /// <summary>
    /// The maximum delay in milliseconds for casting a bait in milliseconds to randomly generate.
    /// </summary>
    public const int MaxCastingDelayMilliseconds = 3000; 

    /// <summary>
    /// The maximum delay in milliseconds for judging a player's performance in milliseconds to randomly generate.
    /// </summary>
    public const int MinJudgingDelayMilliseconds = 1000;
    
    /// <summary>
    /// The maximum delay in milliseconds for judging a player's performance in milliseconds to randomly generate.
    /// </summary>
    public const int MaxJudgingDelayMilliseconds = 2000;

    #endregion
}