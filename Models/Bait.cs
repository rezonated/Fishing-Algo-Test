using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;

namespace FishingAlgoTest.Models;

/// <summary>
/// Represents a bait that can be purchased by the player, thus implementing the IPurchasable interface.
/// The player can buy baits to use for casting the fishing pole, representing for which color the bait is and the cost of the bait.
/// </summary>
public class Bait(FishColor color, int cost) : IPurchasable
{
    /// <summary>
    /// The color of the bait.
    /// </summary>
    public FishColor Color { get; } = color;
    
    /// <summary>
    /// The cost of the bait.
    /// </summary>
    private int Cost { get; } = cost;

    /// <summary>
    /// Buys the bait by the player and the quantity.
    /// </summary>
    /// <param name="player">Player to buy the bait.</param>
    /// <param name="quantity">Quantity of the bait to buy.</param>
    public void Buy(Player player, int quantity)
    {
        var totalCost = Cost * quantity;
        if (player.Gold < totalCost)
        {
            Console.WriteLine("Not enough gold to buy this amount of bait.");
            return;
        }
        
        player.Gold -= totalCost;
        for (var i = 0; i < quantity; i++)
        {
            player.Baits.Add(this);
        }

        Console.WriteLine($"You bought {quantity} {Color} bait(s). You have {player.Gold} gold left.");
    }
}