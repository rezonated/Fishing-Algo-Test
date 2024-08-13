using FishingAlgoTest.Constants;
using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;
using FishingAlgoTest.Models;

namespace FishingAlgoTest.Core;

public class Game(IFishingStrategy fishingStrategy, IPerformanceEvaluationStrategy performanceEvaluationStrategy)
{
    private Player player = new();
    private readonly Pond pond = new();
    private bool skipDay;
    
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

        if (skipDay)
        {
            ResetDay();
            return;
        }

        if (!player.FishingPole.FishSize.Equals(FishSize.Small) || !player.HasOnlyRedBait() || pond.HasSpecificFish(FishSize.Small, FishColor.Red))
        {
            BuyBaits();
            await PlayDayAsync();
        }
        else
        {
            Console.WriteLine("No suitable fish for your pole and bait. Skipping the day.");
            ResetDay();
        }
    }

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

            if (int.TryParse(Console.ReadLine(), out var choice))
            {
                switch (choice)
                {
                    case 1:
                        new FishingPole(FishingPoleType.Small, GameConstants.SmallFishingPoleCost, FishSize.Small).Rent(
                            player);
                        return true;
                    case 2:
                        new FishingPole(FishingPoleType.Medium, GameConstants.MediumFishingPoleCost, FishSize.Medium)
                            .Rent(player);
                        return true;
                    case 3:
                        new FishingPole(FishingPoleType.Big, GameConstants.BigFishingPoleCost, FishSize.Big).Rent(
                            player);
                        return true;
                    case 4:
                        AutoRentAndBuyBaits();
                        return true;
                    case 5:
                        skipDay = true;
                        Console.WriteLine("\nSkipping the day...");
                        return false;
                    case 6:
                        QuitGame();
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Please enter a number between 1 and 6.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a number.");
            }
        }
    }

    private static void QuitGame()
    {
        Console.WriteLine("\nQuitting the game. Goodbye!");
        Environment.Exit(0);
    }

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

        var weightedTotalCost = GameConstants.BestBaitWeight * GetBaitCost(bestOption.color) +
                                GameConstants.OtherBaitWeight * GameConstants.RedBaitCost +
                                GameConstants.OtherBaitWeight * GameConstants.BlueBaitCost +
                                GameConstants.OtherBaitWeight * GameConstants.GreenBaitCost;

        var maxWeightedBaitSet = goldLeft / weightedTotalCost;
        var remainingGold = goldLeft % weightedTotalCost;

        BuyWeightedBaits(bestOption.color, GameConstants.BestBaitWeight * maxWeightedBaitSet);
        BuyWeightedBaits(FishColor.Red, GameConstants.OtherBaitWeight * maxWeightedBaitSet);
        BuyWeightedBaits(FishColor.Blue, GameConstants.OtherBaitWeight * maxWeightedBaitSet);
        BuyWeightedBaits(FishColor.Green, GameConstants.OtherBaitWeight * maxWeightedBaitSet);

        SpendRemainingGoldOnBaits(remainingGold, bestOption.color);
    }

    private void BuyWeightedBaits(FishColor color, int quantity)
    {
        new Bait(color, GetBaitCost(color)).Buy(player, quantity);
    }

    private void SpendRemainingGoldOnBaits(int remainingGold, FishColor bestColor)
    {
        var bestCost = GetBaitCost(bestColor);
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

            if (input is { Length: 2 } && int.TryParse(input[0], out var baitChoice) && int.TryParse(input[1], out var quantity))
            {
                var bait = baitChoice switch
                {
                    1 => new Bait(FishColor.Red, GameConstants.RedBaitCost),
                    2 => new Bait(FishColor.Blue, GameConstants.BlueBaitCost),
                    3 => new Bait(FishColor.Green, GameConstants.GreenBaitCost),
                    _ => null
                };

                if (bait != null)
                {
                    bait.Buy(player, quantity);
                }
                else
                {
                    Console.WriteLine("Invalid bait choice. Please enter 1, 2, or 3.");
                }
            }
            else
            {
                Console.WriteLine(
                    "Invalid input. Please enter the bait number and quantity in the correct format (e.g., '2 10').");
            }
        }
    }

    private async Task PlayDayAsync()
    {
        var didFish = false;

        if (!skipDay)
        {
            await fishingStrategy.FishAsync(pond, player);
            didFish = true;
        }

        PerformanceResult result = await performanceEvaluationStrategy.EvaluateAsync(player, didFish);
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
        }

        Console.WriteLine($"End of Day. You have {player.Gold} gold.");
        ResetDay();
    }

    private void ResetDay()
    {
        player = new Player();
        skipDay = false;
    }

    public async Task StartGameAsync()
    {
        int day = 1;
        while (true)
        {
            await StartDayAsync(day);
            //await PlayDayAsync();
            day++;
            Console.WriteLine("Do you want to continue? (y/n)");
            if (Console.ReadLine().ToLower() == "y") continue;
            QuitGame();
            break;
        }
    }
}