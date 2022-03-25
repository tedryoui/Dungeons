using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory : IItemContainer
{
    private GuiHandler _gui;
    private PlayerBase _playerBase;
    
    [SerializeField] private List<ItemAmount> _items = new List<ItemAmount>();
    public List<ItemAmount> GetPotions => Items.Where(x => x.Item is Potion).ToList();
    [SerializeField] private int _crrPotionPointer;

    public void Init(GuiHandler handler, PlayerBase playerBase)
    {
        _gui = handler;
        _playerBase = playerBase;
        
        _items = new List<ItemAmount>();
        _crrPotionPointer = 0;
        PreLoadBasics();
        
        _gui.PotionHud.Update(this, _crrPotionPointer);
    }

    private void PreLoadBasics()
    {
        var rItems = Resources.LoadAll<Item>("Items/BasePlayerMaterials");
        foreach (Item item in rItems)
            _items.Add(new ItemAmount(){ Item = item, Amount = 0});

        _gui.InventoryHud.Update(this);
    }

    public List<ItemAmount> Items => _items;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) SwitchToTheLeft();
        else if (Input.GetKeyDown(KeyCode.E)) SwitchToTheRight();
        else if (Input.GetKeyDown(KeyCode.F)) UseCrrPotion();
    }
    
    public bool AddItem(Item item, int amount)
    {
        try
        {
            Items.First(x => x.Item.ItemID == item.ItemID).Amount += amount;
            UpdateDueToTheItem(item);
            _gui.DropInfoHud.DisplayNew(item, amount);
            return true;
        }
        catch
        {
            _items.Add(new ItemAmount() { Item = item, Amount = amount});
            UpdateDueToTheItem(item);
            _gui.DropInfoHud.DisplayNew(item, amount);
            return false;
        }
    }

    public bool RemoveItem(Item item, int amount = 1)
    {
        try
        {
            Items.First(x => x.Item.ItemID == item.ItemID).Amount -= amount;

            if (Items.First(x => x.Item.ItemID == item.ItemID).Amount <= 0) 
                Items.First(x => x.Item.ItemID == item.ItemID).Amount = 0;
            
            UpdateDueToTheItem(item);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool CheckForItem(Item item, int amount)
    {
        return Items.Any(x => x.Item.ItemID == item.ItemID && x.Amount >= amount);
    }

    public void SwitchToTheRight()
    {
        var count = GetPotions.Count;
        if (count == 0) return;
        
        _crrPotionPointer = (_crrPotionPointer == 0) ? count - 1 : _crrPotionPointer - 1;

        _gui.PotionHud.Update(this, _crrPotionPointer);
    }
    
    
    public void SwitchToTheLeft()
    {
        var count = GetPotions.Count;
        if (count == 0) return;
        
        _crrPotionPointer = (_crrPotionPointer == count - 1) ? 0 : _crrPotionPointer + 1;

        _gui.PotionHud.Update(this, _crrPotionPointer);
    }

    public void UseCrrPotion()
    {
        if (GetPotions.Count == 0 || GetPotions[_crrPotionPointer].Amount <= 0) return;
        
        (GetPotions[_crrPotionPointer].Item as Potion)?.Use(_playerBase.GetState);
        RemoveItem(GetPotions[_crrPotionPointer].Item);
        
        _gui.PotionHud.Update(this, _crrPotionPointer);
        _gui.PotionHud.PulseCrr();
    }

    private void UpdateDueToTheItem(Item item)
    {
        if(item is Potion)
            _gui.PotionHud.Update(this, _crrPotionPointer);
        else 
            _gui.InventoryHud.Update(this);
    }
}