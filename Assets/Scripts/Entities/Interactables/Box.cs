using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Item;
using UnityEngine;
using Zenject;

public class Box : MonoBehaviour, IDamagable, IItemDroppable
{
    public GameObject destroyParticle;
    
    [Inject] private PlayerBase _playerBase;
    private int health = 2;
    public List<ItemDrop> Drop;

    public AudioSource AudioSource;
    public AudioClip BreakClip;
    public AudioClip HitClip;

    public bool GetDamage(float damage)
    {
        health--;
        var part = Instantiate(destroyParticle);
        part.transform.position = transform.position;
        
        if(health == 0)
        {
            DropItems(_playerBase);
            _playerBase.StartCoroutine(AudioSource.PlayAndDestroy(BreakClip));
            Destroy(gameObject);
        }
        else
        {
           AudioSource.PlayOneShot(HitClip);
        }

        return true;
    }

    public void DropItems<T1, T2>(EntityBase<T1, T2> toWho) where T1 : EntityController where T2 : IEntityState
    {
        foreach (ItemDrop itemDrop in Drop)
            if(Random.Range(0, 1) <= itemDrop.Chance)
                (toWho as PlayerBase)?.Inventory.AddItem(itemDrop.Item, 
                    (int)(itemDrop.Amount /** Random.Range(0f, 0.5f)*/));
    }
}
