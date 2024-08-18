using FishingAlgoTest.Models;

namespace FishingAlgoTest.Interfaces;

/// <summary>
/// Interface for purchasable items.
/// Can be used to represent items that can be purchased by the player.
/// Currently, the game only supports baits that can be purchased by the player.
/// </summary>
public interface IPurchasable
{
    /// <summary>
    /// Buys the purchasable item.
    /// </summary>
    /// <param name="player">Player to buy the item.</param>
    /// <param name="quantity">Quantity of the item to buy.</param>
    void Buy(Player player, int quantity);
}