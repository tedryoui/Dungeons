using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Entities.Player
{
    [Serializable]
    public class PlayerState : IEntityState
    {
        //Fields
        private float _crrHealth;
        [SerializeField] private float _maxHealth;
        private bool _isDead = false;
        private PlayerBase _playerBase;

        //Properties
        private GameObject LoadHealEffect => Resources.Load<GameObject>("Particles/HealEffect");
        public bool IsDead => _isDead;
        public float CrrHealth => _crrHealth;
        public float MaxHealth => _maxHealth;

        //Methods
        public void Initialize<T1, T2>(EntityBase<T1, T2> entityBase)
            where T1 : EntityController where T2 : IEntityState
        {
            _crrHealth = _maxHealth;
            _playerBase = entityBase as PlayerBase;
            
            _playerBase.GuiHandler.PlayerStatusHud.UpdateHealthBar(this);
        }

        public void Heal(float amount)
        {
            if (_crrHealth + amount > _maxHealth) _crrHealth = _maxHealth;
            else _crrHealth += amount;

            _playerBase.GuiHandler.PlayerStatusHud.UpdateHealthBar(this);

            var eff = Object.Instantiate(
                LoadHealEffect,
                _playerBase.transform.position,
                Quaternion.Euler(-90f, 0f, 0f));

            eff.GetComponent<Aura>().SetTarget(_playerBase.transform);
        }

        public void Damage(float amount)
        {
            if (_crrHealth - amount <= 0) Kill();
            else _crrHealth -= amount;

            _playerBase.GuiHandler.PlayerStatusHud.UpdateHealthBar(this);
        }

        public void Revive()
        {
            Heal(MaxHealth);
            _isDead = false;

            _playerBase.GuiHandler.DeadScreenUi.HideDeadScreen();
            _playerBase.Animator.SetTrigger("Revive");
        }

        public void Kill()
        {
            _crrHealth = 0;
            _isDead = true;

            _playerBase.GuiHandler.DeadScreenUi.ShowDeadScreen();
            _playerBase.Animator.SetTrigger("Dead");
        }
    }
}