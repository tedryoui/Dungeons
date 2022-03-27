using System;
using Assets.Scripts.Entities.Player;
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
        private StatusBarHudView _statusBar;
        [SerializeField] private Vector3 _statusBarOffset;

        // Properties
        public float CrrHealth => _crrHealth;
        public float MaxHealth => _maxHealth;
        public bool IsDead => _isDead;
        public GuiHandler GuiHandler => _enemyBase.PlayerBase.GuiHandler;
        public void Initialize<T1, T2>(EntityBase<T1, T2> entityBase) where T1 : EntityController where T2 : IEntityState
        {
            _enemyBase = entityBase as EnemyBase;

            _crrHealth = _maxHealth;
            _statusBar = GuiHandler.StatusBarHud.CreateOne(_enemyBase.transform, _statusBarOffset);
            GuiHandler.StatusBarHud.UpdateOne(_statusBar, this);
        }

        public void Heal(float amount)
        {
            
        }

        public void Damage(float amount)
        {
            if (IsDead) return;
            
            if (_crrHealth - amount <= 0) Kill();
            else _crrHealth -= amount;
            
            GuiHandler.StatusBarHud.UpdateOne(_statusBar, this);
        }

        public void Kill()
        {
            _crrHealth = 0;
            _isDead = true;
            GuiHandler.StatusBarHud.RemoveOne(_statusBar);
            _enemyBase.EnemyStash.DropItems(_enemyBase.PlayerBase);
            
            _enemyBase.Animator.SetTrigger("Dead");
        }
    }
}