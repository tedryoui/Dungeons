
using Assets.Scripts.Entities;

public abstract class Potion : Item
{
    public abstract void Use(IEntityState state);
}