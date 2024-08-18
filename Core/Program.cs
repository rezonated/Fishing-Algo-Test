using FishingAlgoTest.Interfaces;
using FishingAlgoTest.Strategies;

namespace FishingAlgoTest.Core;

/// <summary>
/// Main program entry point.
/// This program is the entry point for the game, which initializes the game and starts the game.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Main program entry point.
    /// This method initializes the game and starts the game.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of starting the game.</returns>
    private static async Task Main()
    {
        IFishingDelayableStrategy fishingDelayableStrategy = new StandardFishingDelayableStrategy();
        
        IPerformanceEvaluationDelayableStrategy performanceEvaluationDelayableStrategy = new StandardPerformanceEvaluationDelayableStrategy();
        
        var game = new Game(fishingDelayableStrategy, performanceEvaluationDelayableStrategy);
        
        await game.StartGameAsync();
    }
}