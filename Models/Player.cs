using FishingAlgoTest.Enums;

namespace FishingAlgoTest.Models;

public class Player
{
    public int Gold { get; set; } = 100;
    public FishingPole? FishingPole { get; set; }
    public List<Bait> Baits { get; } = [];

    public bool HasOnlyRedBait() => Baits.All(b => b.Color == FishColor.Red);
    
    public int GetBaitCount(FishColor color) => Baits.Count(b => b.Color == color);
}