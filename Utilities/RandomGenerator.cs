namespace FishingAlgoTest.Utilities;

public static class RandomGenerator
{
    private static readonly Random Random = new();
    public static int Next(int minValue, int maxValue) => Random.Next(minValue, maxValue);
}