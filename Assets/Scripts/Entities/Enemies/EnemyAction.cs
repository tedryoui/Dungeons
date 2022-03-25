using System;
using UnityEngine;

[Serializable]
public class EnemyAction
{
    public string ActionName;

    [Range(0.1f, 2f)]
    public float Delay;
    
    [Range(0f, 1f)] 
    public float Chance;

    [Space] [Header("Only for damagable")] 
    public float Damage;
    public Vector3 AttackOffset;
    public float AttackSizes;
}