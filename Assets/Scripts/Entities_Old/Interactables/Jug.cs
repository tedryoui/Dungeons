using System;
using System.Collections.Generic;
using Assets.Scripts.Entities.Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[Serializable]
public class ItemDrop
{
    public Item Item;
    public int Amount;
    [Range(0, 1)] public float Chance;
}

public class Jug : MonoBehaviour, IDamagable, IItemDroppable
{
    [Inject] private PlayerBase _playerBase;
    public GameObject destroyParticle;
    public List<ItemDrop> Drop;
    
    public void GetDamage(float damage)
    {
        var part = Instantiate(destroyParticle);
        part.transform.position = transform.position;
        DropItems();
        
        Destroy(gameObject);
    }

    public void DropItems()
    {
        foreach (ItemDrop itemDrop in Drop)
            if(Random.Range(0, 1) <= itemDrop.Chance)
                _playerBase.Inventory.AddItem(itemDrop.Item, 
                    (int)(itemDrop.Amount /** Random.Range(0f, 0.5f)*/));
    }
}