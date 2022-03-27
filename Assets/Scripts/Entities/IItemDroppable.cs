using Assets.Scripts.Entities;

public interface IItemDroppable
{
    public void DropItems<T1, T2>(EntityBase<T1, T2> toWho) where T1 : EntityController where T2 : IEntityState;
}