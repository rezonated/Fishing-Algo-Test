using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;
using FishingAlgoTest.Models;

namespace FishingAlgoTest.Strategies;

/// <summary>
/// Standard performance evaluation delayable strategy.
/// This strategy implements the performance evaluation delayable strategy using standard delays.
/// The strategy is the one that evaluates a player's performance, and applies delays between actions.
/// </summary>
public class StandardPerformanceEvaluationDelayableStrategy : IPerformanceEvaluationDelayableStrategy
{
    /// <summary>
    /// Evaluates a player's performance asynchronously and applies delays between actions.
    /// </summary>
    /// <param name="player">Player to evaluate.</param>
    /// <param name="didFish">Whether the player did fish or not.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of evaluating a player's performance.</returns>
    public async Task<PerformanceResult> EvaluateAsync(Player player, bool didFish)
    {
        if (!didFish)
        {
            return PerformanceResult.Tie;
        }

        var baseStrategy = (IBaseDelayableStrategy)this;

        await baseStrategy.ApplyDelayAsync(GameConstants.MinJudgingDelayMilliseconds, GameConstants.MaxJudgingDelayMilliseconds,
            "Judging your performance...");

        return player.Gold > 100 ? PerformanceResult.Win : player.Gold <= 100 ? PerformanceResult.Lose : PerformanceResult.Tie;
    }
}