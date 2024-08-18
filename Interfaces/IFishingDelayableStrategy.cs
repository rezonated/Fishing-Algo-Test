using FishingAlgoTest.Models;

namespace FishingAlgoTest.Interfaces;

/// <summary>
/// Interface for fishing strategies that can apply delays.
/// Asynchronously fishes the pond and applies delays between actions.
/// </summary>
public interface IFishingDelayableStrategy : IBaseDelayableStrategy
{
    /// <summary>
    /// Fishes the pond asynchronously and applies delays between actions.
    /// </summary>
    /// <param name="pond">Pond to fish.</param>
    /// <param name="player">Player that fishes the pond.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of fishing the pond.</returns>
    Task FishAsync(Pond pond, Player player);
}