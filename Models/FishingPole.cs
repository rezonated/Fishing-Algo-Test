using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;

namespace FishingAlgoTest.Models;

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