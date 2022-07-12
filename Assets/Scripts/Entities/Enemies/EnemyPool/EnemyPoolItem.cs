using System;
using Assets.Scripts.Entities.Enemies;
using UnityEngine;

[Serializable]
public class EnemyPoolItem
{
    public EnemyBase LinkedObject;
    public Vector3 SpawnPoint;
    public bool IsAvailable;

    private Action _onEnemyDestroyed;

    public void Link(EnemyBase @object, Action destroyAction, Vector3 spawnPoint)
    {
        LinkedObject = @object;
        LinkedObject.GetMoveController.Stop();
        LinkedObject.GetState.Remove();
        LinkedObject.gameObject.SetActive(false);
        LinkedObject.transform.position = new Vector3(0f, -100f, 0f);
        
        LinkedObject.OnDestroyAction += Despawn;
        _onEnemyDestroyed += destroyAction;
        SpawnPoint = spawnPoint;
        
        IsAvailable = true;
    }

    public void Spawn(Vector3 spotPoint)
    {
        IsAvailable = false;
        LinkedObject.gameObject.SetActive(true);
        LinkedObject.transform.position = SpawnPoint;
        LinkedObject.GetMoveController.Start(spotPoint);
        LinkedObject.GetState.Start();
    }

    public void Despawn()
    {
        LinkedObject.transform.position = new Vector3(0f, -100f, 0f);
        LinkedObject.GetMoveController.Stop();
        LinkedObject.GetState.Remove();
        LinkedObject.gameObject.SetActive(false);
        
        IsAvailable = true;
        _onEnemyDestroyed();
    }
}