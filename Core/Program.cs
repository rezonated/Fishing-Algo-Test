using FishingAlgoTest.Interfaces;
using FishingAlgoTest.Strategies;

namespace FishingAlgoTest.Core;

internal static class Program
{
    private static async Task Main()
    {
        IFishingStrategy fishingStrategy = new StandardFishingStrategy();
        
        IPerformanceEvaluationStrategy performanceEvaluationStrategy = new StandardPerformanceEvaluationStrategy();
        
        var game = new Game(fishingStrategy, performanceEvaluationStrategy);
        
        await game.StartGameAsync();
    }
}