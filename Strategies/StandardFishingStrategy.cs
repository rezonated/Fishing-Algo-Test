using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;
using FishingAlgoTest.Models;
using FishingAlgoTest.Utilities;

namespace FishingAlgoTest.Strategies;

public class StandardFishingStrategy : IFishingStrategy
{
    public async Task FishAsync(Pond pond, Player player)
    {
        while (player.Baits.Count > 0 && pond.HasFish())
        {
            if (player.FishingPole != null && !pond.CanBeCaughtWithPole(player.FishingPole.FishSize))
            {
                Console.WriteLine(
                    $"The rented pole can only catch {player.FishingPole.FishSize} fish, but no such fish are available in the pond. Skipping fishing.");
                break;
            }

            pond.DisplayAvailableFishes();

            var baitColor = ChooseBait(player);

            if (baitColor == null)
                break;

            Console.WriteLine($"You chose {baitColor} bait. Press the spacebar to cast and pull the pole.");
            WaitForSpacebar();

            await ApplyDelayAsync(GameConstants.MinCastingDelayMilliseconds, GameConstants.MaxCastingDelayMilliseconds,
                "Casting...");

            var bait = player.Baits.First(b => b.Color == baitColor);
            if (player.FishingPole != null)
            {
                var caughtFish = pond.CatchFish(bait.Color, player.FishingPole.FishSize);

                if (caughtFish != null)
                {
                    Console.WriteLine(
                        $"You caught a {caughtFish.Color} {caughtFish.Size} fish worth {caughtFish.Value} gold!");
                    player.Gold += caughtFish.Value;
                }
                else
                {
                    Console.WriteLine("No fish caught this time.");
                }
            }

            player.Baits.Remove(bait);
        }
    }

    private static FishColor? ChooseBait(Player player)
    {
        while (true)
        {
            Console.WriteLine("\nChoose which bait to use:");
            Console.WriteLine($"1. Red bait x{player.GetBaitCount(FishColor.Red)}");
            Console.WriteLine($"2. Blue bait x{player.GetBaitCount(FishColor.Blue)}");
            Console.WriteLine($"3. Green bait x{player.GetBaitCount(FishColor.Green)}");
            
            if (int.TryParse(Console.ReadLine(), out int baitChoice))
            {
                FishColor? chosenBaitColor = baitChoice switch
                {
                    1 => FishColor.Red,
                    2 => FishColor.Blue,
                    3 => FishColor.Green,
                    4 => null,
                    _ => null
                };

                if (baitChoice == 4)
                {
                    Console.WriteLine("You decided to end the day early.");
                    return null;
                }

                if (chosenBaitColor != null && player.Baits.Any(b => b.Color == chosenBaitColor))
                {
                    return chosenBaitColor;
                }

                Console.WriteLine("Invalid choice or no baits of that type available. Please choose again.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number corresponding to the bait type.");
            }
        }
    }

    private static void WaitForSpacebar()
    {
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
        {
        }
    }

    private static async Task ApplyDelayAsync(int minMilliseconds, int maxMilliseconds, string message)
    {
        Console.WriteLine(message);
        var delay = RandomGenerator.Next(minMilliseconds, maxMilliseconds);
        await Task.Delay(delay);
    }
}