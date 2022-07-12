using Assets.Scripts.Entities;
using UnityEngine;

[CreateAssetMenu(menuName = "Potion/StrengthPotion")]
public class StrengthPotion : Potion
{
    public float BonusAmount;
    public float Duration;
    
    public override void Use(IEntityState state) => state.SetStrengthBonus(BonusAmount, Duration);
}