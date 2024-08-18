using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Utilities;

namespace FishingAlgoTest.Models;

/// <summary>
/// Represents a fish that can be caught by the player.
/// The player can catch fish of a specific size and color, representing the size, color, and value of the fish as reward to player in form of gold.
/// </summary>
public class Fish(FishSize size, FishColor color)
{
    /// <summary>
    /// The size of the fish.
    /// </summary>
    public FishSize Size { get; } = size;
    
    /// <summary>
    /// The color of the fish.
    /// </summary>
    public FishColor Color { get; } = color;
    
    /// <summary>
    /// The value of the fish, which is randomly generated based on the size of the fish.
    /// </summary>
    public int Value { get; } = size switch
    {
        FishSize.Small => RandomGenerator.Next(GameConstants.SmallFishMinValue,
            GameConstants.SmallFishMaxValue + 1),
        FishSize.Medium => RandomGenerator.Next(GameConstants.MediumFishMinValue,
            GameConstants.MediumFishMaxValue + 1),
        FishSize.Big => RandomGenerator.Next(GameConstants.BigFishMinValue, GameConstants.BigFishMaxValue + 1),
        _ => GameConstants.SmallFishMinValue
    };
}