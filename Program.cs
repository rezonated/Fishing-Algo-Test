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

    // Interface Segregation
    public interface IRentable
    {
        void Rent(Player player);
    }

    public interface IPurchasable
    {
        void Buy(Player player, int quantity);
    }

    // Fishing Pole Class
    public class FishingPole : IRentable
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

        public void Rent(Player player)
        {
            if (player.Gold >= Cost)
            {
                player.Gold -= Cost;
                player.FishingPole = this;
                Console.WriteLine($"You rented a {Type} Fishing Pole. You have {player.Gold} gold left.");
            }
            else
            {
                Console.WriteLine("Not enough gold to rent this pole.");
            }
        }
    }

    // Bait Class
    public class Bait : IPurchasable
    {
        public FishColor Color { get; }
        public int Cost { get; }

        public Bait(FishColor color, int cost)
        {
            Color = color;
            Cost = cost;
        }

        public void Buy(Player player, int quantity)
        {
            int totalCost = Cost * quantity;
            if (player.Gold >= totalCost)
            {
                player.Gold -= totalCost;
                for (int i = 0; i < quantity; i++)
                {
                    player.Baits.Add(this);
                }
                Console.WriteLine($"You bought {quantity} {Color} bait(s). You have {player.Gold} gold left.");
            }
            else
            {
                Console.WriteLine("Not enough gold to buy this amount of bait.");
            }
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
            Value = size switch
            {
                FishSize.Small => RandomGenerator.Next(GameConstants.SmallFishMinValue, GameConstants.SmallFishMaxValue + 1),
                FishSize.Medium => RandomGenerator.Next(GameConstants.MediumFishMinValue, GameConstants.MediumFishMaxValue + 1),
                FishSize.Big => RandomGenerator.Next(GameConstants.BigFishMinValue, GameConstants.BigFishMaxValue + 1),
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
            // Dynamically generate fish counts based on ranges in GameConstants
            int smallFishCount = RandomGenerator.Next(GameConstants.SmallFishMinCount, GameConstants.SmallFishMaxCount + 1);
            int mediumFishCount = RandomGenerator.Next(GameConstants.MediumFishMinCount, GameConstants.MediumFishMaxCount + 1);
            int bigFishCount = RandomGenerator.Next(GameConstants.BigFishMinCount, GameConstants.BigFishMaxCount + 1);

            // Dynamically generate fish percentages based on ranges in GameConstants
            int redFishPercentage = RandomGenerator.Next(GameConstants.RedFishMinPercentage, GameConstants.RedFishMaxPercentage + 1);
            int blueFishPercentage = RandomGenerator.Next(GameConstants.BlueFishMinPercentage, GameConstants.BlueFishMaxPercentage + 1);
            int greenFishPercentage = Math.Max(0, 100 - (redFishPercentage + blueFishPercentage));

            Console.WriteLine($"Today's forecast: {smallFishCount} small fish, {mediumFishCount} medium fish, and {bigFishCount} big fish.");
            Console.WriteLine($"{redFishPercentage}% are red, {blueFishPercentage}% are blue, and {greenFishPercentage}% are green!");

            _fishes.Clear();
            AddFish(FishSize.Small, smallFishCount, redFishPercentage, blueFishPercentage, greenFishPercentage);
            AddFish(FishSize.Medium, mediumFishCount, redFishPercentage, blueFishPercentage, greenFishPercentage);
            AddFish(FishSize.Big, bigFishCount, redFishPercentage, blueFishPercentage, greenFishPercentage);
        }

        private void AddFish(FishSize size, int count, int redPercentage, int bluePercentage, int greenPercentage)
        {
            for (int i = 0; i < count; i++)
            {
                FishColor color = DetermineFishColor(redPercentage, bluePercentage);
                _fishes.Add(new Fish(size, color));
            }
        }

        private FishColor DetermineFishColor(int redPercentage, int bluePercentage)
        {
            int roll = RandomGenerator.Next(0, 100);
            if (roll < redPercentage) return FishColor.Red;
            if (roll < redPercentage + bluePercentage) return FishColor.Blue;
            return FishColor.Green;
        }

        public Fish CatchFish(FishColor baitColor, FishSize poleSize)
        {
            var fishToCatch = _fishes.FindAll(f => f.Color == baitColor && f.Size == poleSize);

            if (fishToCatch.Count > 0)
            {
                var fish = fishToCatch[RandomGenerator.Next(0, fishToCatch.Count)];
                _fishes.Remove(fish);
                return fish;
            }

            return null;
        }

        public bool HasFish() => _fishes.Count > 0;

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

        public bool CanBeCaughtWithPole(FishSize fishSize)
        {
            return _fishes.Any(f => f.Size == fishSize);
        }

        public bool HasSpecificFish(FishSize size, FishColor color)
        {
            return _fishes.Any(f => f.Size == size && f.Color == color);
        }
    }

    // Player Class
    public class Player
    {
        public int Gold { get; set; } = 100;
        public FishingPole FishingPole { get; set; }
        public List<Bait> Baits { get; private set; } = new List<Bait>();

        public bool HasOnlyRedBait() => Baits.All(b => b.Color == FishColor.Red);

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
    }

    // Fishing Context Strategy Pattern
    public interface IFishingStrategy
    {
        Task FishAsync(Pond pond, Player player);
    }

    public class StandardFishingStrategy : IFishingStrategy
    {
        public async Task FishAsync(Pond pond, Player player)
        {
            while (player.Baits.Count > 0 && pond.HasFish())
            {
                if (!pond.CanBeCaughtWithPole(player.FishingPole.FishSize))
                {
                    Console.WriteLine($"The rented pole can only catch {player.FishingPole.FishSize} fish, but no such fish are available in the pond. Skipping fishing.");
                    break;
                }

                pond.DisplayAvailableFishes();

                var baitColor = ChooseBait(player);

                if (baitColor == null)
                    break;

                Console.WriteLine($"You chose {baitColor} bait. Press the spacebar to cast and pull the pole.");
                WaitForSpacebar();

                await ApplyDelayAsync(GameConstants.MinCastingDelayMilliseconds, GameConstants.MaxCastingDelayMilliseconds, "Casting...");

                var bait = player.Baits.First(b => b.Color == baitColor);
                var caughtFish = pond.CatchFish(bait.Color, player.FishingPole.FishSize);

                if (caughtFish != null)
                {
                    Console.WriteLine($"You caught a {caughtFish.Color} {caughtFish.Size} fish worth {caughtFish.Value} gold!");
                    player.Gold += caughtFish.Value;
                }
                else
                {
                    Console.WriteLine("No fish caught this time.");
                }

                player.Baits.Remove(bait);
            }
        }

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

        private void WaitForSpacebar()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
        }

        private async Task ApplyDelayAsync(int minMilliseconds, int maxMilliseconds, string message)
        {
            Console.WriteLine(message);
            int delay = RandomGenerator.Next(minMilliseconds, maxMilliseconds);
            await Task.Delay(delay);
        }
    }

    // Performance Evaluation Strategy Pattern
    public interface IPerformanceEvaluationStrategy
    {
        Task<PerformanceResult> EvaluateAsync(Player player, bool didFish);
    }

    public class StandardPerformanceEvaluationStrategy : IPerformanceEvaluationStrategy
    {
        public async Task<PerformanceResult> EvaluateAsync(Player player, bool didFish)
        {
            if (!didFish)
            {
                return PerformanceResult.Tie;
            }

            await ApplyDelayAsync(GameConstants.MinJudgingDelayMilliseconds, GameConstants.MaxJudgingDelayMilliseconds, "Judging your performance...");

            return player.Gold > 100 ? PerformanceResult.Win :
                   player.Gold < 100 ? PerformanceResult.Lose :
                   PerformanceResult.Tie;
        }

        private async Task ApplyDelayAsync(int minMilliseconds, int maxMilliseconds, string message)
        {
            Console.WriteLine(message);
            int delay = RandomGenerator.Next(minMilliseconds, maxMilliseconds);
            await Task.Delay(delay);
        }
    }

    // Game Class
    public class Game
    {
        private readonly Player _player;
        private readonly Pond _pond;
        private readonly IFishingStrategy _fishingStrategy;
        private readonly IPerformanceEvaluationStrategy _performanceEvaluationStrategy;
        private bool _skipDay;
        

        public Game(IFishingStrategy fishingStrategy, IPerformanceEvaluationStrategy performanceEvaluationStrategy)
        {
            _player = new Player();
            _pond = new Pond();
            _fishingStrategy = fishingStrategy;
            _performanceEvaluationStrategy = performanceEvaluationStrategy;
            _skipDay = false;
        }

        public async Task StartDayAsync(int day)
        {

            Console.WriteLine($"Day {day} starts!");
            _pond.GenerateForecast();
            Console.WriteLine($"Initial gold: {_player.Gold}");

            if (!ChooseFishingPole())
            {
                ResetDay();
                return;
            }

            if (_skipDay)
            {
                ResetDay();
                return;
            }

            if (!_player.FishingPole.FishSize.Equals(FishSize.Small) || !_player.HasOnlyRedBait() ||
                _pond.HasSpecificFish(FishSize.Small, FishColor.Red))
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

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            new FishingPole(FishingPoleType.Small, GameConstants.SmallFishingPoleCost, FishSize.Small).Rent(_player);
                            return true;
                        case 2:
                            new FishingPole(FishingPoleType.Medium, GameConstants.MediumFishingPoleCost, FishSize.Medium).Rent(_player);
                            return true;
                        case 3:
                            new FishingPole(FishingPoleType.Big, GameConstants.BigFishingPoleCost, FishSize.Big).Rent(_player);
                            return true;
                        case 4:
                            AutoRentAndBuyBaits();
                            return true;
                        case 5:
                            _skipDay = true;
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
            var bestOption = _pond.GetBestFishingOption();

            switch (bestOption.size)
            {
                case FishSize.Small:
                    new FishingPole(FishingPoleType.Small, GameConstants.SmallFishingPoleCost, FishSize.Small).Rent(_player);
                    break;
                case FishSize.Medium:
                    new FishingPole(FishingPoleType.Medium, GameConstants.MediumFishingPoleCost, FishSize.Medium).Rent(_player);
                    break;
                case FishSize.Big:
                    new FishingPole(FishingPoleType.Big, GameConstants.BigFishingPoleCost, FishSize.Big).Rent(_player);
                    break;
            }

            int goldLeft = _player.Gold;

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
            new Bait(color, GetBaitCost(color)).Buy(_player, quantity);
        }

        private void SpendRemainingGoldOnBaits(int remainingGold, FishColor bestColor)
        {
            int bestCost = GetBaitCost(bestColor);
            if (remainingGold >= bestCost)
            {
                new Bait(bestColor, bestCost).Buy(_player, remainingGold / bestCost);
                remainingGold %= bestCost;
            }

            if (remainingGold >= GameConstants.GreenBaitCost)
            {
                new Bait(FishColor.Green, GameConstants.GreenBaitCost).Buy(_player, 1);
                remainingGold -= GameConstants.GreenBaitCost;
            }

            if (remainingGold >= GameConstants.BlueBaitCost)
            {
                new Bait(FishColor.Blue, GameConstants.BlueBaitCost).Buy(_player, 1);
                remainingGold -= GameConstants.BlueBaitCost;
            }

            if (remainingGold >= GameConstants.RedBaitCost)
            {
                new Bait(FishColor.Red, GameConstants.RedBaitCost).Buy(_player, 1);
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
                        bait.Buy(_player, quantity);
                    }
                    else
                    {
                        Console.WriteLine("Invalid bait choice. Please enter 1, 2, or 3.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter the bait number and quantity in the correct format (e.g., '2 10').");
                }
            }
        }

        public async Task PlayDayAsync()
        {
            bool didFish = false;

            if (_player.FishingPole != null && !_skipDay)
            {
                await _fishingStrategy.FishAsync(_pond, _player);
                didFish = true;
            }

            PerformanceResult result = await _performanceEvaluationStrategy.EvaluateAsync(_player, didFish);
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

            Console.WriteLine($"End of Day. You have {_player.Gold} gold.");
            ResetDay();
        }

        private void ResetDay()
        {
            _player.FishingPole = null;
            _player.Baits.Clear();
            _skipDay = false;
        }

        public async Task StartGameAsync()
        {
            int day = 1;
            while (true)
            {
                await StartDayAsync(day);
                await PlayDayAsync();
                day++;
                Console.WriteLine("Do you want to continue? (y/n)");
                if (Console.ReadLine().ToLower() == "y") continue;
                QuitGame();
                break;
            }
        }
    }

    // Main Program Entry Point
    class Program
    {
        static async Task Main(string[] args)
        {
            IFishingStrategy fishingStrategy = new StandardFishingStrategy();
            IPerformanceEvaluationStrategy performanceEvaluationStrategy = new StandardPerformanceEvaluationStrategy();
            Game game = new Game(fishingStrategy, performanceEvaluationStrategy);
            await game.StartGameAsync();
        }
    }

    // Utility Class for Randomization
    public static class RandomGenerator
    {
        private static readonly Random _random = new Random();
        public static int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);
    }
}
