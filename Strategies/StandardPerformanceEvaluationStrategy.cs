using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;
using FishingAlgoTest.Models;
using FishingAlgoTest.Utilities;

namespace FishingAlgoTest.Strategies;

public class StandardPerformanceEvaluationStrategy : IPerformanceEvaluationStrategy
{
    public async Task<PerformanceResult> EvaluateAsync(Player player, bool didFish)
    {
        if (!didFish)
        {
            return PerformanceResult.Tie;
        }

        await ApplyDelayAsync(GameConstants.MinJudgingDelayMilliseconds, GameConstants.MaxJudgingDelayMilliseconds,
            "Judging your performance...");

        return player.Gold > 100 ? PerformanceResult.Win :
            player.Gold <= 100 ? PerformanceResult.Lose :
            PerformanceResult.Tie;
    }

    private async Task ApplyDelayAsync(int minMilliseconds, int maxMilliseconds, string message)
    {
        Console.WriteLine(message);
        int delay = RandomGenerator.Next(minMilliseconds, maxMilliseconds);
        await Task.Delay(delay);
    }
}