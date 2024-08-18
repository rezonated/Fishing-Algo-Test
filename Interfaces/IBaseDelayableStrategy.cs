using FishingAlgoTest.Utilities;

namespace FishingAlgoTest.Interfaces;

/// <summary>
/// Interface for strategies that can apply delays.
/// Can be used to introduce random delays between actions, such as casting a bait or judging
/// a player's performance.
/// </summary>
public interface IBaseDelayableStrategy
{
    /// <summary>
    /// Applies a random delay between the specified min and max milliseconds.
    /// </summary>
    /// <param name="minMilliseconds">The minimum delay in milliseconds.</param>
    /// <param name="maxMilliseconds">The maximum delay in milliseconds.</param>
    /// <param name="message">The message to display before the delay.</param>
    async Task ApplyDelayAsync(int minMilliseconds, int maxMilliseconds, string message)
    {
        Console.WriteLine(message);
        var delay = RandomGenerator.Next(minMilliseconds, maxMilliseconds);
        await Task.Delay(delay);
    }
}