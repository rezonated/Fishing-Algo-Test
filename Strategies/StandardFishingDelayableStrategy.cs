using System.Diagnostics;
using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;
using FishingAlgoTest.Models;

namespace FishingAlgoTest.Strategies;

/// <summary>
/// Standard fishing delayable strategy.
/// This strategy implements the fishing delayable strategy using standard delays.
/// The strategy is the one that actually fishes the pond, casts the bait, and applies delays between actions.
/// </summary>
public class StandardFishingDelayableStrategy : IFishingDelayableStrategy
{
    /// <summary>
    /// Fishes the pond asynchronously and applies delays between actions.
    /// </summary>
    /// <param name="pond">Pond to fish.</param>
    /// <param name="player">Player that fishes the pond.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of fishing the pond.</returns>
    public async Task FishAsync(Pond pond, Player player)
    {
        while (player.Baits.Count > 0 && pond.HasFish())
        {
            Debug.Assert(player.FishingPole != null, "Player should have a fishing pole set before fishing.");
            
            if (!pond.CanBeCaughtWithPole(player.FishingPole.FishSize))
            {
                Console.WriteLine(
                    $"The rented pole can only catch {player.FishingPole.FishSize} fish, but no such fish are available in the pond. Skipping fishing.");
                break;
            }

            pond.DisplayAvailableFishes();

            var baitColor = ChooseBait(player);

            if (baitColor == null)
            {
                break;
            }

            Console.WriteLine($"You chose {baitColor} bait. Press the spacebar to cast and pull the pole.");
            WaitForSpacebar();
            
            var baseStrategy = (IBaseDelayableStrategy)this;

            await baseStrategy.ApplyDelayAsync(GameConstants.MinCastingDelayMilliseconds, GameConstants.MaxCastingDelayMilliseconds,
                "Casting...");

            var bait = player.Baits.First(b => b.Color == baitColor);
            var caughtFish = pond.CatchFish(bait.Color, player.FishingPole.FishSize);

            if (caughtFish == null)
            {
                Console.WriteLine("No fish caught this time.");
                return;
            }

            Console.WriteLine($"You caught a {caughtFish.Color} {caughtFish.Size} fish worth {caughtFish.Value} gold!");
            player.Gold += caughtFish.Value;

            player.Baits.Remove(bait);
        }
    }

    /// <summary>
    /// Chooses a bait to use based on the player's current baits.
    /// </summary>
    /// <param name="player">Player to choose a bait for.</param>
    /// <returns>The color of the bait chosen, or null if the player chooses to end the day.</returns>
    private FishColor? ChooseBait(Player player)
    {
        while (true)
        {
            Console.WriteLine("Choose which bait to use:");
            player.DisplayBaitInventory();
            Console.WriteLine("1. Red bait");
            Console.WriteLine("2. Blue bait");
            Console.WriteLine("3. Green bait");
            Console.WriteLine("4. End the day");

            if (!int.TryParse(Console.ReadLine(), out var baitChoice))
            {
                Console.WriteLine("Invalid input. Please enter a number corresponding to the bait type.");
                return null;
            }

            FishColor? chosenBaitColor = baitChoice switch
            {
                1 => FishColor.Red,
                2 => FishColor.Blue,
                3 => FishColor.Green,
                4 => null, // End the day
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
    }

    /// <summary>
    /// Waits for the spacebar to be pressed, which is used to cast the bait.
    /// </summary>
    private static void WaitForSpacebar()
    {
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
        {
        }
    }
}