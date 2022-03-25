using System.Collections.Generic;

public interface IItemContainer
{
    public List<ItemAmount> Items { get; }
    public bool AddItem(Item item, int amount);
    public bool RemoveItem(Item item, int amount);
    public bool CheckForItem(Item item, int amount);
    
}