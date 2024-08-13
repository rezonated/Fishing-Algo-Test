namespace FishingAlgoTest.Utilities;

public class RandomGenerator
{
    private static readonly Random _random = new Random();
    public static int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
}