using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;

namespace FishingAlgoTest.Models;

public class Bait(FishColor color, int cost) : IPurchasable
{
    public FishColor Color { get; } = color;
    private int Cost { get; } = cost;

    public void Buy(Player player, int quantity)
    {
        var totalCost = Cost * quantity;

        if (player.Gold < totalCost)
        {
            Console.WriteLine("\nNot enough gold to buy this amount of bait.");
            return;
        }

        
        player.Gold -= totalCost;
        for (var i = 0; i < quantity; i++)
        {
            player.Baits.Add(this);
        }

        Console.WriteLine($"\nYou bought x{quantity} {Color} bait(s). You have {player.Gold} gold left.");
    }
}