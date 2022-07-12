using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Entities.Player
{
    [Serializable]
    public class PlayerState : IEntityState
    {
        //Fields
            //Statuses
        private float _crrHealth;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        
            //Status bonuses
        [HideInInspector] public float SpeedBonus;
        [HideInInspector] public float DamageBonus;
        [HideInInspector] public bool IsVulnerable = true;
        
        private bool _isDead = false;
        private PlayerBase _playerBase;
        

        //Properties
        private GameObject LoadHealEffect => Resources.Load<GameObject>("Particles/HealEffect");
        private GameObject LoadStrengthBonusParticle => Resources.Load<GameObject>("Particles/StrengthBonus");
        private GameObject LoadAgilityBonusParticle => Resources.Load<GameObject>("Particles/AgilityBonus");
        public bool IsDead => _isDead;
        public float CrrHealth => _crrHealth;
        public float Damage => _damage + DamageBonus;
        public float Speed => _speed + SpeedBonus;
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

        public bool Injure(float amount)
        {
            if (IsDead || !IsVulnerable) return false;
            
            if (_crrHealth - amount <= 0) Kill();
            else _crrHealth -= amount;

            _playerBase.GuiHandler.PlayerStatusHud.UpdateHealthBar(this);

            return true;
        }

        public void Revive()
        {
            Heal(MaxHealth);

            _playerBase.GuiHandler.DeadScreenUi.HideDeadScreen();
            _playerBase.Animator.SetTrigger("Revive");
        }

        public void FinishRevive()
        {
            _isDead = false;
        }

        public void Kill()
        {
            _crrHealth = 0;
            _isDead = true;

            _playerBase.GuiHandler.DeadScreenUi.ShowDeadScreen();
            _playerBase.Animator.SetTrigger("Dead");
        }

        public void SetAgilityBonus(float amount, float duration) => _playerBase.StartCoroutine(AgilityBonus(amount, duration));

        private IEnumerator AgilityBonus(float amount, float duration)
        {
            var particle = Object.Instantiate(LoadAgilityBonusParticle,
                _playerBase.transform);
            SpeedBonus += amount;
            yield return new WaitForSeconds(duration);
            SpeedBonus -= amount;
            Object.Destroy(particle);
        }
        
        public void SetStrengthBonus(float amount, float duration) => _playerBase.StartCoroutine(StrengthBonus(amount, duration));

        private IEnumerator StrengthBonus(float amount, float duration)
        {
            var particle = Object.Instantiate(LoadStrengthBonusParticle,
                _playerBase.transform);
            DamageBonus += amount;
            yield return new WaitForSeconds(duration);
            DamageBonus -= amount;
            Object.Destroy(particle);
        }
    }
}