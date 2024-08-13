using FishingAlgoTest.Enums;
using FishingAlgoTest.Models;

namespace FishingAlgoTest.Interfaces;

public interface IPerformanceEvaluationStrategy
{
    Task<PerformanceResult> EvaluateAsync(Player player, bool didFish);
}