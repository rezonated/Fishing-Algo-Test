using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;

namespace FishingAlgoTest.Models;

public class FishingPole(FishingPoleType type, int cost, FishSize fishSize) : IRentable
{
    private FishingPoleType Type { get; } = type;
    private int Cost { get; } = cost;
    public FishSize FishSize { get; } = fishSize;

    public void Rent(Player player)
    {
        if (player.Gold < Cost)
        {
            Console.WriteLine("\nNot enough gold to rent this pole.");
            return;
        }
        
        player.Gold -= Cost;
        player.FishingPole = this;
        Console.WriteLine($"\nYou rented a {Type} Fishing Pole. You have {player.Gold} gold left.");
    }
}