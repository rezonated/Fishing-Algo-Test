using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Utilities;

namespace FishingAlgoTest.Models;

// Fish Class
public class Fish
{
    public FishSize Size { get; }
    public FishColor Color { get; }
    public int Value { get; }

    public Fish(FishSize size, FishColor color)
    {
        Size = size;
        Color = color;
        Value = size switch
        {
            FishSize.Small => RandomGenerator.Next(GameConstants.SmallFishMinValue,
                GameConstants.SmallFishMaxValue + 1),
            FishSize.Medium => RandomGenerator.Next(GameConstants.MediumFishMinValue,
                GameConstants.MediumFishMaxValue + 1),
            FishSize.Big => RandomGenerator.Next(GameConstants.BigFishMinValue, GameConstants.BigFishMaxValue + 1),
            _ => GameConstants.SmallFishMinValue
        };
    }
}