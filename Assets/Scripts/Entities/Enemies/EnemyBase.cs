using System;
using Assets.Scripts.Entities.Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Assets.Scripts.Entities.Enemies
{
    public class EnemyBase : EntityBase<EnemyMoveController, EnemyState>, IDamagable
    {
        //Fields
        [SerializeField] private string _indivName;
        [SerializeField] private EnemyStash _enemyStash; 
        
        //Properties
        public EnemyStash EnemyStash => _enemyStash;
        public string Name => _indivName;
        
        //Player reference
        [Inject] public PlayerBase PlayerBase { get; private set; }
        
        //Actions
        public Action OnDestroyAction;
        
        // Components
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }
        public AudioSource AudioSource;

        protected override void EntityStart()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
        }

        protected override void EntityUpdate()
        {
            
        }

        public void OverlapAttackAreaByCrrAction() => MoveController.OverlapAttackAreaByCrrAction();

        protected void FallDown() => StartCoroutine(MoveController.Fall());

        public bool GetDamage(float damage) => GetState.Injure(damage);
        public void PlayClip(AudioClip clip) => this.AudioSource.PlayOneShot(clip);

        void OnDrawGizmosSelected() => MoveController.DrawGizmos(transform);

    }
}