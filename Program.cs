namespace FishingGame
{
    // Constants
    public static class GameConstants
    {
        public const int SmallFishingPoleCost = 5;
        public const int MediumFishingPoleCost = 10;
        public const int BigFishingPoleCost = 15;

        public const int RedBaitCost = 1;
        public const int BlueBaitCost = 2;
        public const int GreenBaitCost = 3;

        public const int BestBaitWeight = 2;
        public const int OtherBaitWeight = 1;

        public const int SmallFishMinValue = 1;
        public const int SmallFishMaxValue = 5;
        public const int MediumFishMinValue = 5;
        public const int MediumFishMaxValue = 10;
        public const int BigFishMinValue = 10;
        public const int BigFishMaxValue = 15;

        // Forecast constants
        public const int SmallFishMinCount = 3;
        public const int SmallFishMaxCount = 12;
        public const int MediumFishMinCount = 2;
        public const int MediumFishMaxCount = 8;
        public const int BigFishMinCount = 1;
        public const int BigFishMaxCount = 6;

        public const int RedFishMinPercentage = 20;
        public const int RedFishMaxPercentage = 50;
        public const int BlueFishMinPercentage = 25;
        public const int BlueFishMaxPercentage = 60;

        public const int MinCastingDelayMilliseconds = 2000; // 2 seconds
        public const int MaxCastingDelayMilliseconds = 3000; // 3 seconds

        public const int MinJudgingDelayMilliseconds = 1000; // 1 second
        public const int MaxJudgingDelayMilliseconds = 2000; // 2 seconds
    }

    // Enums
    public enum FishSize
    {
        Small,
        Medium,
        Big
    }

    public enum FishColor
    {
        Red,
        Blue,
        Green
    }

    public enum FishingPoleType
    {
        Small,
        Medium,
        Big
    }

    public enum PerformanceResult
    {
        Win,
        Lose,
        Tie
    }

    // Fishing Pole Class
    public class FishingPole
    {
        public FishingPoleType Type { get; }
        public int Cost { get; }
        public FishSize FishSize { get; }

        public FishingPole(FishingPoleType type, int cost, FishSize fishSize)
        {
            Type = type;
            Cost = cost;
            FishSize = fishSize;
        }
    }

    // Bait Class
    public class Bait
    {
        public FishColor Color { get; }
        public int Cost { get; }

        public Bait(FishColor color, int cost)
        {
            Color = color;
            Cost = cost;
        }
    }

    // Fish Class
    public class Fish
    {
        public FishSize Size { get; }
        public FishColor Color { get; }
        public int Value { get; }

        public Fish(FishSize size, FishColor color)
        {
            Size = size;
            Color = color;
            Value = GetFishValue(size);
        }

        private int GetFishValue(FishSize size)
        {
            Random rand = new Random();
            return size switch
            {
                FishSize.Small => rand.Next(GameConstants.SmallFishMinValue, GameConstants.SmallFishMaxValue + 1),
                FishSize.Medium => rand.Next(GameConstants.MediumFishMinValue, GameConstants.MediumFishMaxValue + 1),
                FishSize.Big => rand.Next(GameConstants.BigFishMinValue, GameConstants.BigFishMaxValue + 1),
                _ => GameConstants.SmallFishMinValue
            };
        }
    }

    // Pond Class
    public class Pond
    {
        private List<Fish> _fishes;

        public Pond()
        {
            _fishes = new List<Fish>();
        }

        public void GenerateForecast()
        {
            Random rand = new Random();

            // Dynamically generate fish counts based on ranges in GameConstants
            int smallFish = rand.Next(GameConstants.SmallFishMinCount, GameConstants.SmallFishMaxCount + 1);
            int mediumFish = rand.Next(GameConstants.MediumFishMinCount, GameConstants.MediumFishMaxCount + 1);
            int bigFish = rand.Next(GameConstants.BigFishMinCount, GameConstants.BigFishMaxCount + 1);

            // Dynamically generate fish percentages based on ranges in GameConstants
            int redFishPercentage =
                rand.Next(GameConstants.RedFishMinPercentage, GameConstants.RedFishMaxPercentage + 1);
            int blueFishPercentage =
                rand.Next(GameConstants.BlueFishMinPercentage, GameConstants.BlueFishMaxPercentage + 1);
            int totalPercentage = redFishPercentage + blueFishPercentage;

            // Safeguard to ensure valid percentages
            int greenFishPercentage = 100 - totalPercentage;
            if (greenFishPercentage < 0)
            {
                greenFishPercentage = 0;
            }

            Console.WriteLine(
                $"Today we're seeing {smallFish} small fish, {mediumFish} medium fish and {bigFish} big fish.");
            Console.WriteLine(
                $"{redFishPercentage}% of them are red, {blueFishPercentage}% are blue and {greenFishPercentage}% are green!");

            _fishes.Clear();
            AddFish(FishSize.Small, smallFish, redFishPercentage, blueFishPercentage, greenFishPercentage);
            AddFish(FishSize.Medium, mediumFish, redFishPercentage, blueFishPercentage, greenFishPercentage);
            AddFish(FishSize.Big, bigFish, redFishPercentage, blueFishPercentage, greenFishPercentage);
        }

        private void AddFish(FishSize size, int count, int redPercentage, int bluePercentage, int greenPercentage)
        {
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                int roll = rand.Next(0, 100);
                FishColor color = roll < redPercentage ? FishColor.Red :
                    roll < redPercentage + bluePercentage ? FishColor.Blue : FishColor.Green;
                _fishes.Add(new Fish(size, color));
            }
        }

        public Fish CatchFish(FishColor baitColor, FishSize poleSize)
        {
            var fishToCatch = _fishes.FindAll(f => f.Color == baitColor && f.Size == poleSize);

            if (fishToCatch.Count > 0)
            {
                var fish = fishToCatch[new Random().Next(fishToCatch.Count)];
                _fishes.Remove(fish);
                return fish;
            }

            return null;
        }

        public bool HasFish() => _fishes.Count > 0;

        public bool HasSpecificFish(FishSize size, FishColor color)
        {
            return _fishes.Any(f => f.Size == size && f.Color == color);
        }

        public bool CanBeCaughtWithPole(FishSize fishSize)
        {
            return _fishes.Any(f => f.Size == fishSize);
        }

        public void DisplayAvailableFishes()
        {
            var groupedFishes = _fishes
                .GroupBy(f => new { f.Color, f.Size })
                .Select(group => new
                {
                    Color = group.Key.Color,
                    Size = group.Key.Size,
                    Count = group.Count()
                });

            Console.WriteLine("Available fishes in the pond:");
            foreach (var fish in groupedFishes)
            {
                Console.WriteLine($"{fish.Color} {fish.Size} fish: {fish.Count} available");
            }
        }

        public (FishSize size, FishColor color) GetBestFishingOption()
        {
            var bestFishGroup = _fishes
                .GroupBy(f => new { f.Color, f.Size })
                .OrderByDescending(group => group.Count())
                .FirstOrDefault();

            return bestFishGroup != null
                ? (bestFishGroup.Key.Size, bestFishGroup.Key.Color)
                : (FishSize.Small, FishColor.Red);
        }
    }

    // Player Class
    public class Player
    {
        public int Gold { get; private set; } = 100;
        public FishingPole FishingPole { get; private set; }
        public List<Bait> Baits { get; private set; } = new List<Bait>();

        public void RentFishingPole(FishingPole pole)
        {
            if (Gold >= pole.Cost)
            {
                FishingPole = pole;
                Gold -= pole.Cost;
                Console.WriteLine($"You rented a {pole.Type} Fishing Pole. You have {Gold} gold left.");
            }
            else
            {
                Console.WriteLine("Not enough gold to rent this pole.");
            }
        }

        public void BuyBait(Bait bait, int quantity)
        {
            int totalCost = bait.Cost * quantity;
            if (Gold >= totalCost)
            {
                for (int i = 0; i < quantity; i++)
                {
                    Baits.Add(bait);
                }

                Gold -= totalCost;
                Console.WriteLine($"You bought {quantity} {bait.Color} bait(s). You have {Gold} gold left.");
                DisplayBaitInventory();
            }
            else
            {
                Console.WriteLine("Not enough gold to buy this amount of bait.");
            }
        }

        public void DisplayBaitInventory()
        {
            Console.WriteLine("Current bait inventory:");
            var baitGroups = Baits.GroupBy(b => b.Color)
                .Select(group => new
                {
                    Color = group.Key,
                    Count = group.Count()
                });

            foreach (var group in baitGroups)
            {
                Console.WriteLine($"{group.Color} bait: {group.Count} available");
            }
        }

        public bool HasOnlyRedBait()
        {
            return Baits.All(b => b.Color == FishColor.Red);
        }

        public void Fish(Pond pond)
{
    while (Baits.Count > 0 && pond.HasFish())
    {
        if (!pond.CanBeCaughtWithPole(FishingPole.FishSize))
        {
            Console.WriteLine($"The rented pole can only catch {FishingPole.FishSize} fish, but no such fish are available in the pond. Skipping fishing.");
            break;
        }

        pond.DisplayAvailableFishes();

        FishColor? chosenBaitColor = null;
        while (chosenBaitColor == null)
        {
            Console.WriteLine("Choose which bait to use:");

            // Display the bait inventory as part of the fishing prompt
            var baitInventory = Baits
                .GroupBy(b => b.Color)
                .Select(group => new
                {
                    Color = group.Key,
                    Count = group.Count()
                }).ToList();

            Console.WriteLine($"1. Red bait ({baitInventory.FirstOrDefault(b => b.Color == FishColor.Red)?.Count ?? 0} available)");
            Console.WriteLine($"2. Blue bait ({baitInventory.FirstOrDefault(b => b.Color == FishColor.Blue)?.Count ?? 0} available)");
            Console.WriteLine($"3. Green bait ({baitInventory.FirstOrDefault(b => b.Color == FishColor.Green)?.Count ?? 0} available)");
            Console.WriteLine("4. End the day");

            if (int.TryParse(Console.ReadLine(), out int baitChoice))
            {
                chosenBaitColor = baitChoice switch
                {
                    1 => FishColor.Red,
                    2 => FishColor.Blue,
                    3 => FishColor.Green,
                    4 => null, // Special case for ending the day
                    _ => null
                };

                if (baitChoice == 4)
                {
                    Console.WriteLine("You decided to end the day early.");
                    break;
                }

                if (chosenBaitColor == null || !Baits.Any(b => b.Color == chosenBaitColor))
                {
                    Console.WriteLine("Invalid choice or no baits of that type available. Please choose again.");
                    chosenBaitColor = null;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number corresponding to the bait type.");
            }
        }

        if (chosenBaitColor == null) break;

        Console.WriteLine($"You chose {chosenBaitColor} bait. Press the spacebar to cast and pull the pole.");
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar)
        {
        }

// Adding suspense delay before determining if a fish is caught
        int castingDelay = new Random().Next(GameConstants.MinCastingDelayMilliseconds,
            GameConstants.MaxCastingDelayMilliseconds);
        Console.WriteLine("Casting...");
        Thread.Sleep(castingDelay);

        var bait = Baits.First(b => b.Color == chosenBaitColor);
        var caughtFish = pond.CatchFish(bait.Color, FishingPole.FishSize);

        if (caughtFish != null)
        {
            Console.WriteLine($"You caught a {caughtFish.Color} {caughtFish.Size} fish worth {caughtFish.Value} gold!");
            Gold += caughtFish.Value;
        }
        else
        {
            Console.WriteLine("No fish caught this time.");
        }

        Baits.Remove(bait);
    }
}


        public PerformanceResult EvaluatePerformance(bool didFish)
        {
            if (!didFish)
            {
                // If the player skipped the day without fishing, the result is always a tie.
                return PerformanceResult.Tie;
            }

            // Suspense delay before showing the result
            int judgingDelay = new Random().Next(GameConstants.MinJudgingDelayMilliseconds,
                GameConstants.MaxJudgingDelayMilliseconds);
            Console.WriteLine("Judging your performance...");
            System.Threading.Thread.Sleep(judgingDelay);

            if (Gold > 100)
                return PerformanceResult.Win;
            else if (Gold <= 100)
                return PerformanceResult.Lose;
            else
                return PerformanceResult.Tie;
        }

    }

    // Game Class
    public class Game
    {
        private Player _player;
        private Pond _pond;
        private bool _skipDay;
        private bool _resetWithoutBaitBuying;

        public Game()
        {
            _player = new Player();
            _pond = new Pond();
            _skipDay = false;
            _resetWithoutBaitBuying = false;
        }

        public void StartDay(int day)
        {
            if (_resetWithoutBaitBuying)
            {
                _resetWithoutBaitBuying = false;
                Console.WriteLine($"Day {day} starts! (Tie from previous day, skipping bait buying)");
                Console.WriteLine($"Initial gold: {_player.Gold}");
                return;
            }

            Console.WriteLine($"Day {day} starts!");
            _pond.GenerateForecast();

            // State the initial gold after the forecast
            Console.WriteLine($"Initial gold: {_player.Gold}");

            ChooseFishingPole();

            if (_skipDay)
            {
                ResetDay();
                return;
            }

            BuyBaits();
            Console.WriteLine(
                $"You have rented a {_player.FishingPole.Type} Fishing Pole and have {_player.Gold} gold left.");
            Console.WriteLine($"You have {_player.Baits.Count} baits.");

            if (_player.FishingPole.FishSize == FishSize.Small && _player.HasOnlyRedBait() &&
                !_pond.HasSpecificFish(FishSize.Small, FishColor.Red))
            {
                Console.WriteLine(
                    "You have only small fishing pole and red bait, but there are no small red fish in the pond. Skipping fishing.");
                PlayDay();
                return;
            }
        }


        private void ChooseFishingPole()
        {
            bool validChoice = false;
            while (!validChoice)
            {
                Console.WriteLine("Choose your fishing pole:");
                Console.WriteLine($"1. Small fishing pole, {GameConstants.SmallFishingPoleCost} gold");
                Console.WriteLine($"2. Medium fishing pole, {GameConstants.MediumFishingPoleCost} gold");
                Console.WriteLine($"3. Big fishing pole, {GameConstants.BigFishingPoleCost} gold");
                Console.WriteLine("4. Auto rent pole and buy baits based on forecast");
                Console.WriteLine("5. Skip the day");
                Console.WriteLine("6. Quit the game");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    validChoice = true;
                    switch (choice)
                    {
                        case 1:
                            _player.RentFishingPole(new FishingPole(FishingPoleType.Small,
                                GameConstants.SmallFishingPoleCost, FishSize.Small));
                            break;
                        case 2:
                            _player.RentFishingPole(new FishingPole(FishingPoleType.Medium,
                                GameConstants.MediumFishingPoleCost, FishSize.Medium));
                            break;
                        case 3:
                            _player.RentFishingPole(new FishingPole(FishingPoleType.Big,
                                GameConstants.BigFishingPoleCost, FishSize.Big));
                            break;
                        case 4:
                            AutoRentAndBuyBaits();
                            break;
                        case 5:
                            _skipDay = true; // Skip the day
                            Console.WriteLine("Skipping the day...");
                            Console.WriteLine("Performance resulted in a Tie.");
                            _resetWithoutBaitBuying = true;
                            //ResetDay();
                            return; // Skip further processing for this day
                        case 6:
                            Console.WriteLine("Quitting the game. Goodbye!");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                            validChoice = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private void AutoRentAndBuyBaits()
        {
            var bestOption = _pond.GetBestFishingOption();

            switch (bestOption.size)
            {
                case FishSize.Small:
                    _player.RentFishingPole(new FishingPole(FishingPoleType.Small, GameConstants.SmallFishingPoleCost,
                        FishSize.Small));
                    break;
                case FishSize.Medium:
                    _player.RentFishingPole(new FishingPole(FishingPoleType.Medium, GameConstants.MediumFishingPoleCost,
                        FishSize.Medium));
                    break;
                case FishSize.Big:
                    _player.RentFishingPole(new FishingPole(FishingPoleType.Big, GameConstants.BigFishingPoleCost,
                        FishSize.Big));
                    break;
            }

            int goldLeft = _player.Gold;
            int totalBaitCost = GameConstants.RedBaitCost + GameConstants.BlueBaitCost + GameConstants.GreenBaitCost;

            int weightedTotalCost = GameConstants.BestBaitWeight * GetBaitCost(bestOption.color) +
                                    GameConstants.OtherBaitWeight * GameConstants.RedBaitCost +
                                    GameConstants.OtherBaitWeight * GameConstants.BlueBaitCost +
                                    GameConstants.OtherBaitWeight * GameConstants.GreenBaitCost;

            int maxWeightedBaitSet = goldLeft / weightedTotalCost;
            int remainingGold = goldLeft % weightedTotalCost;

            BuyWeightedBaits(bestOption.color, GameConstants.BestBaitWeight * maxWeightedBaitSet);
            BuyWeightedBaits(FishColor.Red, GameConstants.OtherBaitWeight * maxWeightedBaitSet, bestOption.color);
            BuyWeightedBaits(FishColor.Blue, GameConstants.OtherBaitWeight * maxWeightedBaitSet, bestOption.color);
            BuyWeightedBaits(FishColor.Green, GameConstants.OtherBaitWeight * maxWeightedBaitSet, bestOption.color);

            SpendRemainingGoldOnBaits(remainingGold, bestOption.color);
        }

        private void BuyWeightedBaits(FishColor color, int quantity, FishColor bestColor = FishColor.Red)
        {
            _player.BuyBait(new Bait(color, GetBaitCost(color)), quantity);
        }

        private void SpendRemainingGoldOnBaits(int remainingGold, FishColor bestColor)
        {
            int bestCost = GetBaitCost(bestColor);
            if (remainingGold >= bestCost)
            {
                _player.BuyBait(new Bait(bestColor, bestCost), remainingGold / bestCost);
                remainingGold %= bestCost;
            }

            if (remainingGold >= GameConstants.GreenBaitCost)
            {
                _player.BuyBait(new Bait(FishColor.Green, GameConstants.GreenBaitCost), 1);
                remainingGold -= GameConstants.GreenBaitCost;
            }

            if (remainingGold >= GameConstants.BlueBaitCost)
            {
                _player.BuyBait(new Bait(FishColor.Blue, GameConstants.BlueBaitCost), 1);
                remainingGold -= GameConstants.BlueBaitCost;
            }

            if (remainingGold >= GameConstants.RedBaitCost)
            {
                _player.BuyBait(new Bait(FishColor.Red, GameConstants.RedBaitCost), 1);
            }
        }

        private int GetBaitCost(FishColor color)
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
            while (_player.Gold > 0)
            {
                Console.WriteLine("Buy your baits:");
                Console.WriteLine($"1. Red bait, {GameConstants.RedBaitCost} gold");
                Console.WriteLine($"2. Blue bait, {GameConstants.BlueBaitCost} gold");
                Console.WriteLine($"3. Green bait, {GameConstants.GreenBaitCost} gold");
                Console.WriteLine("Enter bait number and quantity (e.g., '2 10'):");

                string[] input = Console.ReadLine().Split(' ');

                if (input.Length == 2 && int.TryParse(input[0], out int baitChoice) &&
                    int.TryParse(input[1], out int quantity))
                {
                    Bait bait = baitChoice switch
                    {
                        1 => new Bait(FishColor.Red, GameConstants.RedBaitCost),
                        2 => new Bait(FishColor.Blue, GameConstants.BlueBaitCost),
                        3 => new Bait(FishColor.Green, GameConstants.GreenBaitCost),
                        _ => null
                    };

                    if (bait != null)
                    {
                        _player.BuyBait(bait, quantity);
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

        public void PlayDay()
        {
            bool didFish = false;

            if (_player.FishingPole != null && !_skipDay)
            {
                _player.Fish(_pond);
                didFish = true; // Player started fishing
            }

            PerformanceResult result = _player.EvaluatePerformance(didFish);
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
                    _resetWithoutBaitBuying = true;
                    ResetDay();
                    return; // Skip further processing
            }

            Console.WriteLine($"End of Day. You have {_player.Gold} gold.");
            ResetDay();
        }

        private void ResetDay()
        {
            _player = new Player();
            _pond = new Pond();
            _skipDay = false;
        }

        public void StartGame()
        {
            int day = 1;
            while (true)
            {
                StartDay(day);
                PlayDay();
                day++;
                Console.WriteLine("Do you want to continue? (y/n)");
                if (Console.ReadLine().ToLower() != "y") break;
            }
        }
    }

    // Main Program Entry Point
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();
        }
    }
}