namespace FishingAlgoTest.Utilities;

/// <summary>
/// Static utility class for generating random numbers.
/// Provides a static method for generating random numbers within a specified range using the <see cref="Random"/> class's <see cref="Random.Next(int, int)"/> method.
/// </summary>
public static class RandomGenerator
{
    private static readonly Random Random = new();
    public static int Next(int minValue, int maxValue) => Random.Next(minValue, maxValue);
}