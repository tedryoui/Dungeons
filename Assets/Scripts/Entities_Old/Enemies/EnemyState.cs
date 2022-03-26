using System;
using Assets.Scripts.Entities_Old;

[Serializable]
public class EnemyState : IEntityState
{
    public float MaxHealth;
    public float CrrHealth;

    private Enemy _baseEntity;
    public void Init(Enemy enemy)
    {
        _baseEntity = enemy;
    }
    public void Heal(float amount)
    {
        throw new NotImplementedException();
    }

    public void Damage(float amount)
    {
        CrrHealth -= amount;

        if (CrrHealth <= 0)
        {
            _baseEntity.Kill();
            CrrHealth = 0;
        }
    }
}