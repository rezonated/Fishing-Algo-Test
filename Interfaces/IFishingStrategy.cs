using FishingAlgoTest.Models;

namespace FishingAlgoTest.Interfaces;

public interface IFishingStrategy
{
    Task FishAsync(Pond pond, Player player);
}