using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Item;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{

    [Serializable]
    public class PlayerInventory : EntityController, IItemContainer
    {
        private PlayerBase _playerBase;

        protected override void Initialize<T1, T2>(EntityBase<T1, T2> _entityBase)
        {
            _playerBase = _entityBase as PlayerBase;
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) SwitchToTheLeft();
            else if (Input.GetKeyDown(KeyCode.E)) SwitchToTheRight();
            else if (Input.GetKeyDown(KeyCode.F)) UseCrrPotion();
        }

        [SerializeField] private List<ItemAmount> _items = new List<ItemAmount>();
        [SerializeField] private int _crrPotionPointer;

        public List<ItemAmount> Items => _items;
        public List<ItemAmount> GetPotions => Items.Where(x => x.Item is Potion).ToList();

        public bool AddItem(Item.Item item, int amount)
        {
            try
            {
                Items.First(x => x.Item.ItemID == item.ItemID).Amount += amount;
                UpdateDueToTheItem(item);
                _playerBase.GuiHandler.DropInfoHud.DisplayNew(item, amount);
                return true;
            }
            catch
            {
                _items.Add(new ItemAmount() {Item = item, Amount = amount});
                UpdateDueToTheItem(item);
                _playerBase.GuiHandler.DropInfoHud.DisplayNew(item, amount);
                return false;
            }
        }

        public bool RemoveItem(Item.Item item, int amount = 1)
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

        public bool CheckForItem(Item.Item item, int amount)
        {
            return Items.Any(x => x.Item.ItemID == item.ItemID && x.Amount >= amount);
        }

        public void SwitchToTheLeft()
        {
            var count = GetPotions.Count;
            if (count == 0) return;

            _crrPotionPointer = (_crrPotionPointer == 0) ? count - 1 : _crrPotionPointer - 1;

            _playerBase.GuiHandler.PotionHud.Update(this, _crrPotionPointer);
        }


        public void SwitchToTheRight()
        {
            var count = GetPotions.Count;
            if (count == 0) return;

            _crrPotionPointer = (_crrPotionPointer == count - 1) ? 0 : _crrPotionPointer + 1;

            _playerBase.GuiHandler.PotionHud.Update(this, _crrPotionPointer);
        }

        public void UseCrrPotion()
        {
            if (GetPotions.Count == 0 || GetPotions[_crrPotionPointer].Amount <= 0) return;

            (GetPotions[_crrPotionPointer].Item as Potion)?.Use(_playerBase.GetState);
            RemoveItem(GetPotions[_crrPotionPointer].Item);

            _playerBase.GuiHandler.PotionHud.Update(this, _crrPotionPointer);
            _playerBase.GuiHandler.PotionHud.PulseCrr();
        }

        private void UpdateDueToTheItem(Item.Item item)
        {
            if (item is Potion)
                _playerBase.GuiHandler.PotionHud.Update(this, _crrPotionPointer);
            else
                _playerBase.GuiHandler.InventoryHud.Update(this);
        }
    }
}