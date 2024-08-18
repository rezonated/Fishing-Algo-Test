using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Utilities;

namespace FishingAlgoTest.Models;

/// <summary>
/// Represents a pond that can have fishes.
/// The pond can have fishes to be caught by the player's fishing pole, representing the fishes that can be caught by the player's fishing pole.
/// The pond can also have a forecast of the fishes that will be available in the pond based on the fishing pole the player has, the baits the player has, and the fishes the player has - all of which will always be generated randomly and summed up to 100.
/// </summary>
public class Pond
{
    /// <summary>
    /// The fishes in the pond.
    /// </summary>
    private readonly List<Fish> fishes = [];

    /// <summary>
    /// Generates the forecast of the fishes in the pond based the game constants configuration.
    /// </summary>
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

        Console.WriteLine($"Today's forecast: {smallFishCount} small fish, {mediumFishCount} medium fish, and {bigFishCount} big fish.");
        Console.WriteLine($"{redFishPercentage}% are red, {blueFishPercentage}% are blue, and {greenFishPercentage}% are green!");

        fishes.Clear();
        
        AddFish(FishSize.Small, smallFishCount, redFishPercentage, blueFishPercentage);
        AddFish(FishSize.Medium, mediumFishCount, redFishPercentage, blueFishPercentage);
        AddFish(FishSize.Big, bigFishCount, redFishPercentage, blueFishPercentage);
    }

    /// <summary>
    /// Adds fishes to the pond based on the fish size, count, red percentage, and blue percentage.
    /// </summary>
    /// <param name="size">The size of the fish.</param>
    /// <param name="count">The count of the fish.</param>
    /// <param name="redPercentage">The red percentage of the fish.</param>
    /// <param name="bluePercentage">The blue percentage of the fish.</param>
    private void AddFish(FishSize size, int count, int redPercentage, int bluePercentage)
    {
        for (var i = 0; i < count; i++)
        {
            var color = DetermineFishColor(redPercentage, bluePercentage);
            fishes.Add(new Fish(size, color));
        }
    }

    /// <summary>
    /// Determines the color of the fish based on the red and blue percentages.
    /// </summary>
    /// <param name="redPercentage">The red percentage of the fish.</param>
    /// <param name="bluePercentage">The blue percentage of the fish.</param>
    /// <returns>The color of the fish.</returns>
    private static FishColor DetermineFishColor(int redPercentage, int bluePercentage)
    {
        var roll = RandomGenerator.Next(0, 100);
        if (roll < redPercentage) return FishColor.Red;
        return roll < redPercentage + bluePercentage ? FishColor.Blue : FishColor.Green;
    }

    /// <summary>
    /// Catches a fish based on the bait color and the size of the fishing pole.
    /// </summary>
    /// <param name="baitColor">The color of the bait.</param>
    /// <param name="poleSize">The size of the fishing pole.</param>
    /// <returns>The fish that was caught, or null if no fish was caught.</returns>
    public Fish? CatchFish(FishColor baitColor, FishSize poleSize)
    {
        var fishToCatch = fishes.FindAll(fish => fish.Color == baitColor && fish.Size == poleSize);

        if (fishToCatch.Count <= 0)
        {
            return null;
        }

        var randomFishToCatch = fishToCatch[RandomGenerator.Next(0, fishToCatch.Count)];
        fishes.Remove(randomFishToCatch);
        
        return randomFishToCatch;
    }

    /// <summary>
    /// Checks if the pond has fishes.
    /// </summary>
    /// <returns>True if the pond has fishes, false otherwise.</returns>
    public bool HasFish() => fishes.Count > 0;

    /// <summary>
    /// Displays the available fishes in the pond.
    /// </summary>
    public void DisplayAvailableFishes()
    {
        var groupedFishes = fishes
            .GroupBy(fish => new { fish.Color, fish.Size })
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

    /// <summary>
    /// Gets the best fishing option based on the fishes in the pond, calculated by getting which fish color and size group has the most fishes.
    /// </summary>
    /// <returns></returns>
    public (FishSize size, FishColor color) GetBestFishingOption()
    {
        var bestFishGroup = fishes
            .GroupBy(fish => new { fish.Color, fish.Size }).MaxBy(group => group.Count());

        return bestFishGroup != null
            ? (bestFishGroup.Key.Size, bestFishGroup.Key.Color)
            : (FishSize.Small, FishColor.Red);
    }

    /// <summary>
    /// Checks if the fishes in the pond can be caught with the specified fish size.
    /// </summary>
    /// <param name="fishSize">The size of the fish.</param>
    /// <returns>True if the fishes in the pond can be caught with the specified fish size, false otherwise.</returns>
    public bool CanBeCaughtWithPole(FishSize fishSize)
    {
        return fishes.Any(fish => fish.Size == fishSize);
    }

    /// <summary>
    /// Checks if the fishes in the pond have a specific fish size and color.
    /// </summary>
    /// <param name="size">The size of the fish.</param>
    /// <param name="color">The color of the fish.</param>
    /// <returns>True if the fishes in the pond have a specific fish size and color, false otherwise.</returns>
    public bool HasSpecificFish(FishSize size, FishColor color)
    {
        return fishes.Any(fish => fish.Size == size && fish.Color == color);
    }
}