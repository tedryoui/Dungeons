using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Arena;
using Assets.Scripts.Entities.Enemies;
using UnityEngine;
using Zenject;
using Object = System.Object;

public class EnemyPool : MonoBehaviour
{
    public List<GameObject> Prefabs;
    [SerializeField] private List<EnemyPoolItem> _poolItems;

    protected Action OnEnemyDestroyed;
    [Inject] private DiContainer Container;

    protected void Create()
    {
        _poolItems = new List<EnemyPoolItem>();
        
        foreach (GameObject prefab in Prefabs)
        {
            var enemy = Container.InstantiatePrefab(prefab, transform);
            _poolItems.Add(new EnemyPoolItem());
            _poolItems.Last().Link(enemy.GetComponent<EnemyBase>(), OnEnemyDestroyed, transform.position);
        }
    }

    public EnemyPoolItem Take(string name) => _poolItems.First(x => x.LinkedObject.Name == name && x.IsAvailable);

    public bool HasEnemiesOfType(int count, string name) => _poolItems.
        Where(x => x.LinkedObject.Name == name && x.IsAvailable).
        ToList().
        Count <= count;
}