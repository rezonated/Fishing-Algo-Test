using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;

namespace FishingAlgoTest.Models;

/// <summary>
/// Represents a fishing pole that can be rented by the player, thus implementing the IRentable interface.
/// The player can rent fishing poles to cast them and catch fish, representing the type, cost, and size of the fishing pole.
/// </summary>
/// <param name="type">The type of the fishing pole.</param>
/// <param name="cost">The cost of the fishing pole.</param>
/// <param name="fishSize">The size of the fish that can be caught by the fishing pole.</param>
public class FishingPole(FishingPoleType type, int cost, FishSize fishSize) : IRentable
{
    /// <summary>
    /// The type of the fishing pole.
    /// </summary>
    private FishingPoleType Type { get; } = type;
    
    /// <summary>
    /// The cost of the fishing pole.
    /// </summary>
    private int Cost { get; } = cost;
    
    /// <summary>
    /// The size of the fish that can be caught by the fishing pole.
    /// </summary>
    public FishSize FishSize { get; } = fishSize;

    /// <summary>
    /// Rents the fishing pole by the player.
    /// </summary>
    /// <param name="player">Player to rent the fishing pole.</param>
    public void Rent(Player player)
    {
        if (player.Gold < Cost)
        {
            Console.WriteLine("Not enough gold to rent this pole.");
            return;
        }

        player.Gold -= Cost;
        player.FishingPole = this;
        Console.WriteLine($"You rented a {Type} Fishing Pole. You have {player.Gold} gold left.");
    }
}