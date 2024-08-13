using FishingAlgoTest.Enums;

namespace FishingAlgoTest.Models;

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