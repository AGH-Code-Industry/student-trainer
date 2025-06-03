using UnityEngine;
using Zenject;

public class ItemUsingService : IInitializable
{
    [Inject] readonly PlayerService playerService;

    public void Initialize()
    {
        
    }

    public void UseItem(ItemPreset item)
    {
        if(item.GetType() == typeof(HealingItemPreset))
        {
            HealingItemPreset healing = (HealingItemPreset)item;
            UseHeal(healing.healAmount);
        }
    }

    void UseHeal(float amount)
    {
        playerService.Health += amount;
    }
}
