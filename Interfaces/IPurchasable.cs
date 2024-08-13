using FishingAlgoTest.Models;

namespace FishingAlgoTest.Interfaces;

public interface IPurchasable
{
    void Buy(Player player, int quantity);
}