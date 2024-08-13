using FishingAlgoTest.Enums;
using FishingAlgoTest.Interfaces;

namespace FishingAlgoTest.Models;

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