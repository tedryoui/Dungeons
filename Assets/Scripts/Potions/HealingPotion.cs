using Assets.Scripts.Entities;
using UnityEngine;

[CreateAssetMenu(menuName = "Potion/HealingPotion")]
public class HealingPotion : Potion
{
    public float HealingAmount;
    
    public override void Use(IEntityState state) => state.Heal(HealingAmount);
}