using System;
using System.Linq;


[Serializable]
public class InventoryHudViewController
{
    public InventoryHudView View;

    public void Update(IItemContainer itemContainer)
    {
        View.ScrapText.text = $"Scrap x{itemContainer.Items.FirstOrDefault(x => x.Item.ItemID == 0)?.Amount.ToString()}";
        View.HerbsText.text = $"Herbs x{itemContainer.Items.FirstOrDefault(x => x.Item.ItemID == 1)?.Amount.ToString()}";
        View.CatalistText.text = $"Catalizer x{itemContainer.Items.FirstOrDefault(x => x.Item.ItemID == 2)?.Amount.ToString()}";
    }
}
