using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Utilities;

namespace FishingAlgoTest.Models;

public class Fish(FishSize size, FishColor color)
{
    public FishSize Size { get; } = size;
    public FishColor Color { get; } = color;
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