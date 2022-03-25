using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Object = UnityEngine.Object;

[Serializable]
public class DropInfoHudViewController
{

    public DropInfoHudView View;

    private List<DropInfo> _drops = new List<DropInfo>();
    
    public void DisplayNew(Item item, int amount)
    {
        if(TryToAdd(item, amount)) return;
        
        var drop = GameObject.Instantiate(View.DropInfoPrefab, View.transform).GetComponent<DropInfo>();
        
        drop.SetDefaults(new ItemAmount() {Item = item, Amount = amount});
        _drops.Add(drop);
    }

    public bool TryToAdd(Item item, int amount)
    {
        ClearDropsList();

        if (_drops.Any(x => x.Item.Item.ItemID == item.ItemID))
        {
            var drop = _drops.First(x => x.Item.Item.ItemID == item.ItemID);
            _drops.Remove(drop);
            drop.UpdateAmount(amount);
            _drops.Add(drop);

            return true;
        }
        
        return false;
    }

    public void ClearDropsList()
    {
        for (int i = 0; i < _drops.Count; i++)
            if(_drops[i] == null) _drops.RemoveAt(i);
    }
}
