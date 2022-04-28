using System;
using Assets.Scripts.Entities.Player;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    [Serializable]
    public class EnemyState : IEntityState
    {
        // Fields
            //Statuses
        [SerializeField] private float _crrHealth;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _speed;
        //[SerializeField] private float _damage;
        
            //Status bonuses
        [HideInInspector] public float SpeedBonus;
        [HideInInspector] public float DamageBonus;
        [HideInInspector] public bool IsVulnerable = true;
        
        private bool _isDead;
        private EnemyBase _enemyBase;
        private StatusBarHudView _statusBar;
        [SerializeField] private Vector3 _statusBarOffset;

        // Properties
        public float CrrHealth => _crrHealth;
        public float MaxHealth => _maxHealth;
        public float Damage { get; }
        public float Speed => _speed;
        public bool IsDead => _isDead;
        
        private GuiHandler GuiHandler => _enemyBase.PlayerBase.GuiHandler;
        public void Initialize<T1, T2>(EntityBase<T1, T2> entityBase) where T1 : EntityController where T2 : IEntityState
        {
            _enemyBase = entityBase as EnemyBase;

            Start();
        }

        public void Heal(float amount)
        {
            
        }

        public bool Injure(float amount)
        {
            if (IsDead || !IsVulnerable) return false;
            
            if (_crrHealth - amount <= 0) Kill();
            else _crrHealth -= amount;
            
            GuiHandler.StatusBarHud.UpdateOne(_statusBar, this);

            return true;
        }

        public void Kill()
        {
            _crrHealth = 0;
            _isDead = true;
            _enemyBase.EnemyStash.DropItems(_enemyBase.PlayerBase);
            
            _enemyBase.Animator.SetTrigger("Dead");
        }

        public void SetAgilityBonus(float amount, float duration)
        {
            
        }

        public void SetStrengthBonus(float amount, float duration)
        {
            
        }

        public void Remove()
        {
            GuiHandler.StatusBarHud.RemoveOne(_statusBar);
            _statusBar = null;
        }

        public void Start()
        {
            _crrHealth = _maxHealth;
            _isDead = false;
            _statusBar = GuiHandler.StatusBarHud.CreateOne(_enemyBase.transform, _statusBarOffset);
            GuiHandler.StatusBarHud.UpdateOne(_statusBar, this);
        }
    }
}