using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    [Serializable]
    public class EnemyState : IEntityState
    {
        // Fields
        [SerializeField] private float _crrHealth;
        [SerializeField] private float _maxHealth;
        private bool _isDead;
        private EnemyBase _enemyBase;

        // Properties
        public float CrrHealth => _crrHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _isDead;
        public void Initialize<T1, T2>(EntityBase<T1, T2> entityBase) where T1 : EntityController where T2 : IEntityState
        {
            _enemyBase = entityBase as EnemyBase;

            _crrHealth = _maxHealth;
        }

        public void Heal(float amount)
        {
            
        }

        public void Damage(float amount)
        {
            if (_crrHealth - amount <= 0) Kill();
            else _crrHealth -= amount;
        }

        public void Kill()
        {
            _crrHealth = 0;
            _isDead = true;
            
            _enemyBase.Animator.SetTrigger("Dead");
        }
    }
}