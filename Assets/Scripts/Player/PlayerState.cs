using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class PlayerState : IEntityState
{

    private float _crrHealth;
    public float CrrHealth
    {
        get => _crrHealth;
        private set
        {
            _crrHealth = value;
            _gui.PlayerStatusHud.UpdateHealthBar(this);
        }
    }

    public float MaxHealth;

    private Transform _player;
    private GuiHandler _gui;
    private GameObject LoadHealEffect => Resources.Load<GameObject>("Particles/HealEffect");

    public void Init(Transform position, GuiHandler gui)
    {
        _player = position;
        _gui = gui;

        CrrHealth = MaxHealth;
    }
    
    public void Heal(float amount)
    {
        if (CrrHealth + amount > MaxHealth) CrrHealth = MaxHealth;
        else CrrHealth += amount;
        
        _gui.PlayerStatusHud.UpdateHealthBar(this);

        var eff = Object.Instantiate(LoadHealEffect, _player.position, Quaternion.Euler(-90f, 0f, 0f));
        eff.GetComponent<Aura>().SetTarget(_player);
    }

    public void Damage(float amount)
    {
        if (CrrHealth - amount <= 0)
        {
            CrrHealth = 0;

            _player.gameObject.GetComponent<PlayerBase>().Kill();
        }
        else CrrHealth -= amount;
        
        _gui.PlayerStatusHud.UpdateHealthBar(this);
    }

    public bool IsDead() => CrrHealth == 0;

    public void RestoreHp() => Heal(MaxHealth);
}