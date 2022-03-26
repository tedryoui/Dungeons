using System;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public abstract class EntityBase<T1, T2> : MonoBehaviour
        where T1 : EntityController
        where T2 : IEntityState

    {
        [SerializeField] protected T1 MoveController;
        [SerializeField] protected T2 StateController;

        public T1 GetMoveController => MoveController;
        public T2 GetState => StateController;

        protected Action OnUpdateAction;

        void Start()
        {
            EntityStart();
            
            //Initializing controllers
            (MoveController as EntityController)?.SafeInitialize(ref OnUpdateAction, this);
            (StateController as IEntityState)?.Initialize(this);
        }

        protected abstract void EntityStart();

        void Update()
        {
            if (StateController.IsDead) return;

            //Calling bounded methods "Update" from every controller
            OnUpdateAction?.Invoke();

            EntityUpdate();
        }

        protected abstract void EntityUpdate();
    }
}