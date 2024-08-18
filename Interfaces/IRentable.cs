using FishingAlgoTest.Models;

namespace FishingAlgoTest.Interfaces;

/// <summary>
/// Interface for rentable items.
/// Can be used to represent items that can be rented by the player.
/// Currently, the game only supports fishing poles that can be rented by the player.
/// </summary>
public interface IRentable
{
    /// <summary>
    /// Rents the rentable item.
    /// </summary>
    /// <param name="player">Player to rent the item.</param>
    void Rent(Player player);
}