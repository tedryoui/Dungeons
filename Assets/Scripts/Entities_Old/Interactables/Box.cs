using System.Collections.Generic;
using Assets.Scripts.Entities.Player;
using UnityEngine;
using Zenject;

public class Box : MonoBehaviour, IDamagable, IItemDroppable
{
    public GameObject destroyParticle;
    
    [Inject] private PlayerBase _playerBase;
    private int health = 2;
    public List<ItemDrop> Drop;

    public void GetDamage(float damage)
    {
        health--;
        var part = Instantiate(destroyParticle);
        part.transform.position = transform.position;
        
        if(health == 0)
        {
            DropItems();
            Destroy(gameObject);
        }
    }

    public void DropItems()
    {
        foreach (ItemDrop itemDrop in Drop)
            if(Random.Range(0, 1) <= itemDrop.Chance)
                _playerBase.Inventory.AddItem(itemDrop.Item, 
                    (int)(itemDrop.Amount * Random.Range(0.5f, 1f) + 0.5f));
    }
}
