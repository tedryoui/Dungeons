namespace Assets.Scripts.Entities
{
    public interface IEntityState
    {
        public float CrrHealth { get; }
        public float MaxHealth { get; }
        public bool IsDead { get; }

        public void Initialize<T1, T2>(EntityBase<T1, T2> entityBase)
            where T1 : EntityController where T2 : IEntityState;

        public void Heal(float amount);
        public void Damage(float amount);
        public void Kill();
    }
}