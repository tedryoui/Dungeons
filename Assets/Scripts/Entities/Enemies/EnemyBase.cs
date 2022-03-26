using Assets.Scripts.Entities.Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Assets.Scripts.Entities.Enemies
{
    public class EnemyBase : EntityBase<EnemyMoveController, EnemyState>, IDamagable
    {
        //Spot
        [SerializeField] public EnemySpot Spot;
        
        //Player reference
        [Inject] public PlayerBase PlayerBase { get; private set; }
        
        // Components
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }
        
        protected override void EntityStart()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }

        protected override void EntityUpdate()
        {
        }

        public void OverlapAttackAreaByActionName() => MoveController.OverlapAttackAreaByCrrAction();

        protected void FallDown() =>
            StartCoroutine(MoveController.Fall());
        public void GetDamage(float damage)
        {
            GetState.Damage(damage);
        }
    }
}