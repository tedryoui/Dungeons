using System;

namespace Assets.Scripts.Entities
{
    public abstract class EntityController
    {
        public void SafeInitialize<T1, T2>(ref Action updateAction, EntityBase<T1, T2> _entityBase) where T1 : EntityController where T2 : IEntityState
        {
            updateAction += Update;
            
            Initialize(_entityBase);
        }

        protected abstract void Initialize<T1, T2>(EntityBase<T1, T2> _entityBase)
            where T1 : EntityController where T2 : IEntityState;

        protected abstract void Update();
    }
}