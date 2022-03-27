using System;
using System.Collections.Generic;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Item;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entities.Enemies
{
    [Serializable]
    public class EnemyStash : IItemDroppable
    {

        [SerializeField] private List<ItemDrop> _drops;


        public void DropItems<T1, T2>(EntityBase<T1, T2> toWho) where T1 : EntityController where T2 : IEntityState
        {
            foreach (ItemDrop drop in _drops)
                if(Random.Range(0, 1) <= drop.Chance)
                    (toWho as PlayerBase)?.Inventory.AddItem(drop.Item, 
                        (int)(drop.Amount * Random.Range(0.5f, 1f) + 0.5f));
        }
    }
}