using FishingAlgoTest.Enums;

namespace FishingAlgoTest.Models;

/// <summary>
/// Represents a player that can have a fishing pole and baits.
/// The player can have a fishing pole and baits to cast the fishing pole and use the baits for casting also their current gold which initially is 100.
/// The player can also check if they have only red baits, which is used for sanity check later on checking if the player has only red baits and if they can actually fish at that day based on the fishing pole they have, available fishes, and the baits they have.
/// </summary>
public class Player
{
    /// <summary>
    /// The current gold of the player, which initially is 100.
    /// </summary>
    public int Gold { get; set; } = 100;
    
    /// <summary>
    /// The current fishing pole of the player. Can be null if the player does not have a fishing pole.
    /// </summary>
    public FishingPole? FishingPole { get; set; }
    
    /// <summary>
    /// The current baits of the player
    /// </summary>
    public List<Bait> Baits { get; } = [];

    /// <summary>
    /// Checks if the player has only red baits for sanity check checking if the player can actually fish at that day.
    /// </summary>
    /// <returns>True if the player has only red baits, false otherwise.</returns>
    public bool HasOnlyRedBait() => Baits.All(bait => bait.Color == FishColor.Red);

    /// <summary>
    /// Displays the current baits player has.
    /// </summary>
    public void DisplayBaitInventory()
    {
        Console.WriteLine("Current bait inventory:");
        var baitGroups = Baits.GroupBy(bait => bait.Color)
            .Select(group => new
            {
                Color = group.Key,
                Count = group.Count()
            });

        foreach (var group in baitGroups)
        {
            Console.WriteLine($"{group.Color} bait: {group.Count} available");
        }
    }
}