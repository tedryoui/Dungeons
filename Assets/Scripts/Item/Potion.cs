
using Assets.Scripts.Entities;
using Assets.Scripts.Item;

public abstract class Potion : Item
{
    public abstract void Use(IEntityState state);
}