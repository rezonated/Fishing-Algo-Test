using System.Diagnostics;
using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;
using FishingAlgoTest.Models;

namespace FishingAlgoTest.Core;

/// <summary>
/// The actual core game class.
/// This class represents the game, which includes the player, the pond, and the strategies for fishing and evaluating performance.
/// It also includes methods for starting the game, displaying the game menu, and handling the game logic.
/// The whole game is implemented using the fishing delayable strategy and the performance evaluation delayable strategy.
/// The fishing delayable strategy is used for fishing the pond, casting the bait, and applying delays between actions.
/// The performance evaluation delayable strategy is used for evaluating a player's performance, and applying delays between actions.
/// The way it is implemented is that the game starts with the player choosing a fishing pole and baits, and the pond is generated based on the game constants configuration.
/// The game then enters a loop where the player can fish, cast their bait, and evaluate their performance.
/// </summary>
/// <param name="fishingDelayableStrategy">The fishing delayable strategy to use for fishing and casting the bait.</param>
/// <param name="performanceEvaluationDelayableStrategy">The performance evaluation delayable strategy to use for evaluating the player's performance.</param>
public class Game(IFishingDelayableStrategy fishingDelayableStrategy, IPerformanceEvaluationDelayableStrategy performanceEvaluationDelayableStrategy)
{
    /// <summary>
    /// The player of the game.
    /// </summary>
    private Player player = new();
    
    /// <summary>
    /// The pond of the game.
    /// </summary>
    private readonly Pond pond = new();
    
    /// <summary>
    /// Whether they skipped the day.
    /// </summary>
    private bool skippedDay;
    
    /// <summary>
    /// Starts the game day asynchronously.
    /// </summary>
    /// <param name="day">The day to start.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of starting the game day.</returns>
    private async Task StartDayAsync(int day)
    {
        Console.WriteLine($"Day {day} starts!");
        pond.GenerateForecast();
        Console.WriteLine($"Initial gold: {player.Gold}");

        if (!ChooseFishingPole())
        {
            ResetDay();
            return;
        }

        if (skippedDay)
        {
            ResetDay();
            return;
        }
        
        Debug.Assert(player.FishingPole != null, "Player should have a fishing pole set before fishing.");
        
        if (player.FishingPole.FishSize.Equals(FishSize.Small) && player.HasOnlyRedBait() &&
            !pond.HasSpecificFish(FishSize.Small, FishColor.Red))
        {
            Console.WriteLine("No suitable fish for your pole and bait. Skipping the day.");
            ResetDay();
            return;
        }

        BuyBaits();
        await PlayDayAsync();
    }

    /// <summary>
    /// Chooses a fishing pole.
    /// </summary>
    /// <returns>True if the player chooses to skip the day, false otherwise.</returns>
    private bool ChooseFishingPole()
    {
        while (true)
        {
            Console.WriteLine("\nChoose your fishing pole:");
            Console.WriteLine("1. Small fishing pole");
            Console.WriteLine("2. Medium fishing pole");
            Console.WriteLine("3. Big fishing pole");
            Console.WriteLine("4. Auto rent pole and buy baits based on forecast");
            Console.WriteLine("5. Skip the day");
            Console.WriteLine("6. Quit the game");

            if (!int.TryParse(Console.ReadLine(), out var choice))
            {
                Console.WriteLine("\nInvalid input. Please enter a number.");
                return false;
            }

            switch (choice)
            {
                case 1: // Rent a small fishing pole
                    new FishingPole(FishingPoleType.Small, GameConstants.SmallFishingPoleCost, FishSize.Small).Rent(
                        player);
                    return true;
                case 2: // Rent a medium fishing pole
                    new FishingPole(FishingPoleType.Medium, GameConstants.MediumFishingPoleCost, FishSize.Medium)
                        .Rent(player);
                    return true;
                case 3: // Rent a big fishing pole
                    new FishingPole(FishingPoleType.Big, GameConstants.BigFishingPoleCost, FishSize.Big).Rent(
                        player);
                    return true;
                case 4: // Auto rent and buy baits based on forecast
                    AutoRentAndBuyBaits();
                    return true;
                case 5: // Skip the day
                    skippedDay = true;
                    Console.WriteLine("\nSkipping the day...");
                    return false;
                case 6: // Quit the game
                    QuitGame();
                    break;
                default:
                    Console.WriteLine("\nInvalid choice. Please enter a number between 1 and 6.");
                    break;
            }
        }
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    private static void QuitGame()
    {
        Console.WriteLine("\nQuitting the game. Goodbye!");
        Environment.Exit(0);
    }

    /// <summary>
    /// Auto rents and buys baits.
    /// How it works:
    /// 1. Gets the best fishing option based on the fishes in the pond.
    /// 2. Rents the fishing pole based on the best fishing option.
    /// 3. Buys the baits based on the best fishing option with the calculated weight under the game constants configuration for best bait weight and other bait weight until the player has ran out of gold.
    /// Only then, the player can actually start fishing.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void AutoRentAndBuyBaits()
    {
        var bestOption = pond.GetBestFishingOption();

        switch (bestOption.size)
        {
            case FishSize.Small:
                new FishingPole(FishingPoleType.Small, GameConstants.SmallFishingPoleCost, FishSize.Small)
                    .Rent(player);
                break;
            case FishSize.Medium:
                new FishingPole(FishingPoleType.Medium, GameConstants.MediumFishingPoleCost, FishSize.Medium)
                    .Rent(player);
                break;
            case FishSize.Big:
                new FishingPole(FishingPoleType.Big, GameConstants.BigFishingPoleCost, FishSize.Big).Rent(player);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var goldLeft = player.Gold;

        // Weighted total cost of the best bait is calculated as the weight of the best bait multiplied by the cost of the best bait, plus the weights of the other baits multiplied by their respective costs.
        // This is done to ensure that the player has a balanced amount of baits to use for casting the fishing pole but still has enough gold to buy the other baits.
        var weightedTotalCost = GameConstants.BestBaitWeight * GetBaitCost(bestOption.color) + GameConstants.OtherBaitWeight * GameConstants.RedBaitCost + GameConstants.OtherBaitWeight * GameConstants.BlueBaitCost + GameConstants.OtherBaitWeight * GameConstants.GreenBaitCost;

        var maxWeightedBaitSet = goldLeft / weightedTotalCost;
        var remainingGold = goldLeft % weightedTotalCost;

        // Buy the best bait and the other baits based on the weight and quantity, then spend the remaining gold on the other baits to balance the total cost and allow player start fishing.
        BuyWeightedBaits(bestOption.color, GameConstants.BestBaitWeight * maxWeightedBaitSet);
        BuyWeightedBaits(FishColor.Red, GameConstants.OtherBaitWeight * maxWeightedBaitSet);
        BuyWeightedBaits(FishColor.Blue, GameConstants.OtherBaitWeight * maxWeightedBaitSet);
        BuyWeightedBaits(FishColor.Green, GameConstants.OtherBaitWeight * maxWeightedBaitSet);

        SpendRemainingGoldOnBaits(remainingGold, bestOption.color);
    }

    /// <summary>
    /// Buys the weighted baits based on the color and quantity.
    /// </summary>
    /// <param name="color">The color of the bait.</param>
    /// <param name="quantity">The quantity of the bait to buy.</param>
    private void BuyWeightedBaits(FishColor color, int quantity)
    {
        new Bait(color, GetBaitCost(color)).Buy(player, quantity);
    }

    /// <summary>
    /// Spends the remaining gold on the best bait and other baits to balance the total cost and allow player start fishing.
    /// </summary>
    /// <param name="remainingGold">The remaining gold to spend.</param>
    /// <param name="bestColor">The color of the best bait.</param>
    private void SpendRemainingGoldOnBaits(int remainingGold, FishColor bestColor)
    {
        var bestCost = GetBaitCost(bestColor);
        // The whole idea of this is to buy the best bait and the other baits based on the weight and quantity, then spend the remaining gold on the other baits to balance the total cost and allow player start fishing.
        // This is done by checking if the remaining gold is greater than or equal to the cost of the best bait, and if so, buying the best bait and the other baits based on the weight and quantity.
        //Then it checks for all the other bait color types.
        if (remainingGold >= bestCost)
        {
            new Bait(bestColor, bestCost).Buy(player, remainingGold / bestCost);
            remainingGold %= bestCost;
        }

        if (remainingGold >= GameConstants.GreenBaitCost)
        {
            new Bait(FishColor.Green, GameConstants.GreenBaitCost).Buy(player, 1);
            remainingGold -= GameConstants.GreenBaitCost;
        }

        if (remainingGold >= GameConstants.BlueBaitCost)
        {
            new Bait(FishColor.Blue, GameConstants.BlueBaitCost).Buy(player, 1);
            remainingGold -= GameConstants.BlueBaitCost;
        }

        if (remainingGold >= GameConstants.RedBaitCost)
        {
            new Bait(FishColor.Red, GameConstants.RedBaitCost).Buy(player, 1);
        }
    }

    /// <summary>
    /// Gets the cost of a bait based on its color.
    /// </summary>
    /// <param name="color">The color of the bait.</param>
    /// <returns>The cost of the bait.</returns>
    private static int GetBaitCost(FishColor color)
    {
        return color switch
        {
            FishColor.Red => GameConstants.RedBaitCost,
            FishColor.Blue => GameConstants.BlueBaitCost,
            FishColor.Green => GameConstants.GreenBaitCost,
            _ => GameConstants.RedBaitCost
        };
    }

    /// <summary>
    /// Buys the baits.
    /// </summary>
    private void BuyBaits()
    {
        while (player.Gold > 0)
        {
            Console.WriteLine("Buy your baits:");
            Console.WriteLine($"1. Red bait, {GameConstants.RedBaitCost} gold");
            Console.WriteLine($"2. Blue bait, {GameConstants.BlueBaitCost} gold");
            Console.WriteLine($"3. Green bait, {GameConstants.GreenBaitCost} gold");
            Console.WriteLine("Enter bait number and quantity (e.g., '2 10'):");

            var input = Console.ReadLine()?.Split(' ');

            if (input is not { Length: 2 } || !int.TryParse(input[0], out var baitChoice) ||
                !int.TryParse(input[1], out var quantity))
            {
                Console.WriteLine("Invalid input. Please enter the bait number and quantity in the correct format (e.g., '2 10').");
                return;
            }

            var bait = baitChoice switch
            {
                1 => new Bait(FishColor.Red, GameConstants.RedBaitCost),
                2 => new Bait(FishColor.Blue, GameConstants.BlueBaitCost),
                3 => new Bait(FishColor.Green, GameConstants.GreenBaitCost),
                _ => null
            };

            if (bait == null)
            {
                Console.WriteLine("Invalid bait choice. Please enter 1, 2, or 3.");
                return;
            }

            bait.Buy(player, quantity);
        }
    }

    /// <summary>
    /// Plays the day asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of playing the day.</returns>
    private async Task PlayDayAsync()
    {
        var didFish = false;

        // If the player did not choose to skip the day, they can go fishing!
        if (!skippedDay)
        {
            await fishingDelayableStrategy.FishAsync(pond, player);
            didFish = true;
        }

        // After fishing, the player can evaluate their performance.
        var result = await performanceEvaluationDelayableStrategy.EvaluateAsync(player, didFish);
        switch (result)
        {
            case PerformanceResult.Win:
                Console.WriteLine("You won! You earned more than 100 gold.");
                break;
            case PerformanceResult.Lose:
                Console.WriteLine("You lost! You earned 100 gold or less.");
                break;
            case PerformanceResult.Tie:
                Console.WriteLine("It's a tie! You kept the same gold.");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Console.WriteLine($"End of Day. You have {player.Gold} gold.");
        ResetDay();
    }

    /// <summary>
    /// Resets the day.
    /// </summary>
    private void ResetDay()
    {
        player = new Player();
        skippedDay = false;
    }

    /// <summary>
    /// Starts the game asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of starting the game.</returns>
    public async Task StartGameAsync()
    {
        // Each day, the player can start fishing. Currently, there are no persistent data, so each new run of the game will start with day 1 and a fresh fish forecast.
        var day = 1;
        while (true)
        {
            await StartDayAsync(day);
            day++;
            
            Console.WriteLine("Do you want to continue? (y/n)");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                continue;
            }

            QuitGame();
            break;
        }
    }
}