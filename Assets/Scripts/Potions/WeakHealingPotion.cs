using Assets.Scripts.Entities;
using UnityEngine;

[CreateAssetMenu(menuName = "Potion/WeakHealingPotion")]
public class WeakHealingPotion : Potion
{
    public float HealingAmount;
    
    public override void Use(IEntityState state)
    {
        state.Heal(HealingAmount);
    }
}