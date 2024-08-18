using FishingAlgoTest.Enums;
using FishingAlgoTest.Models;

namespace FishingAlgoTest.Interfaces;

/// <summary>
/// Interface for performance evaluation strategies that can apply delays.
/// Asynchronously evaluates a player's performance and applies delays between actions.
/// </summary>
public interface IPerformanceEvaluationDelayableStrategy : IBaseDelayableStrategy
{
    /// <summary>
    /// Evaluates a player's performance asynchronously and applies delays between actions.
    /// </summary>
    /// <param name="player">Player to evaluate.</param>
    /// <param name="didFish">Whether the player did fish or not.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of evaluating a player's performance.</returns>
    Task<PerformanceResult> EvaluateAsync(Player player, bool didFish);
}