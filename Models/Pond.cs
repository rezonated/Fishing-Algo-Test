using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Utilities;

namespace FishingAlgoTest.Models;

public class Pond
{
    private readonly List<Fish> fishes = [];

    public void GenerateForecast()
    {
        // Dynamically generate fish counts based on ranges in GameConstants
        var smallFishCount = RandomGenerator.Next(GameConstants.SmallFishMinCount, GameConstants.SmallFishMaxCount + 1);
        var mediumFishCount =
            RandomGenerator.Next(GameConstants.MediumFishMinCount, GameConstants.MediumFishMaxCount + 1);
        var bigFishCount = RandomGenerator.Next(GameConstants.BigFishMinCount, GameConstants.BigFishMaxCount + 1);

        // Dynamically generate fish percentages based on ranges in GameConstants
        var redFishPercentage =
            RandomGenerator.Next(GameConstants.RedFishMinPercentage, GameConstants.RedFishMaxPercentage + 1);
        var blueFishPercentage =
            RandomGenerator.Next(GameConstants.BlueFishMinPercentage, GameConstants.BlueFishMaxPercentage + 1);
        var greenFishPercentage = Math.Max(0, 100 - (redFishPercentage + blueFishPercentage));

        Console.WriteLine(
            $"Today's forecast: {smallFishCount} small fish, {mediumFishCount} medium fish, and {bigFishCount} big fish.");
        Console.WriteLine(
            $"{redFishPercentage}% are red, {blueFishPercentage}% are blue, and {greenFishPercentage}% are green!");

        fishes.Clear();
        AddFish(FishSize.Small, smallFishCount, redFishPercentage, blueFishPercentage);
        AddFish(FishSize.Medium, mediumFishCount, redFishPercentage, blueFishPercentage);
        AddFish(FishSize.Big, bigFishCount, redFishPercentage, blueFishPercentage);
    }

    private void AddFish(FishSize size, int count, int redPercentage, int bluePercentage)
    {
        for (var i = 0; i < count; i++)
        {
            var color = DetermineFishColor(redPercentage, bluePercentage);
            fishes.Add(new Fish(size, color));
        }
    }

    private FishColor DetermineFishColor(int redPercentage, int bluePercentage)
    {
        var roll = RandomGenerator.Next(0, 100);
        if (roll < redPercentage) return FishColor.Red;
        return roll < redPercentage + bluePercentage ? FishColor.Blue : FishColor.Green;
    }

    public Fish CatchFish(FishColor baitColor, FishSize poleSize)
    {
        var fishToCatch = fishes.FindAll(f => f.Color == baitColor && f.Size == poleSize);

        if (fishToCatch.Count <= 0)
        {
            return null;
        }

        var fish = fishToCatch[RandomGenerator.Next(0, fishToCatch.Count)];
        fishes.Remove(fish);
        
        return fish;

    }

    public bool HasFish() => fishes.Count > 0;

    public void DisplayAvailableFishes()
    {
        var groupedFishes = fishes
            .GroupBy(f => new { f.Color, f.Size })
            .Select(group => new
            {
                group.Key.Color,
                group.Key.Size,
                Count = group.Count()
            });

        Console.WriteLine("Available fishes in the pond:");
        foreach (var fish in groupedFishes)
        {
            Console.WriteLine($"{fish.Color} {fish.Size} fish: {fish.Count} available");
        }
    }

    public (FishSize size, FishColor color) GetBestFishingOption()
    {
        var bestFishGroup = fishes
            .GroupBy(f => new { f.Color, f.Size }).MaxBy(group => group.Count());

        return bestFishGroup != null
            ? (bestFishGroup.Key.Size, bestFishGroup.Key.Color)
            : (FishSize.Small, FishColor.Red);
    }

    public bool CanBeCaughtWithPole(FishSize fishSize)
    {
        return fishes.Any(f => f.Size == fishSize);
    }

    public bool HasSpecificFish(FishSize size, FishColor color)
    {
        return fishes.Any(f => f.Size == size && f.Color == color);
    }
}