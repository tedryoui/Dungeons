using Assets.Scripts.Entities;
using UnityEngine;

[CreateAssetMenu(menuName = "Potion/AgilityPotion")]
public class AgilityPotion : Potion
{
    public float BonusAmount;
    public float Duration;
    
    public override void Use(IEntityState state) => state.SetAgilityBonus(BonusAmount, Duration);
}