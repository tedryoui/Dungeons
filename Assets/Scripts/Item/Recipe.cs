using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemAmount
{
    public Item Item;
    public int Amount;
}

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public List<ItemAmount> IN;
    public List<ItemAmount> OUT;

    public bool CanCraft(IItemContainer container)
    {
        foreach (ItemAmount itemAmount in IN)
            if (!container.CheckForItem(itemAmount.Item, itemAmount.Amount)) return false;
        
        return true;
    }

    public bool Craft(IItemContainer fromItemContainer, IItemContainer toItemContainer)
    {
        if (CanCraft(fromItemContainer))
        {
            foreach (ItemAmount itemAmount in IN)
                fromItemContainer.RemoveItem(itemAmount.Item, itemAmount.Amount);

            foreach (ItemAmount itemAmount in OUT)
                toItemContainer.AddItem(itemAmount.Item, itemAmount.Amount);

            return true;
        }

        return false;
    }
}
