using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Item;

namespace Assets.Scripts.GUI.RecipeItemsHud
{
    
    [Serializable]
    public class RecipeItemsHudViewController
    {
        public RecipeItemsHudView View;

        public void Show(List<ItemAmount> itemsAmount)
        {
            var herbs = itemsAmount.First(x => x.Item.ItemID == 2);
            var catalist = itemsAmount.First(x => x.Item.ItemID == 1);
            var scrap = itemsAmount.First(x => x.Item.ItemID == 0);

            View.ScrapText.transform.parent.gameObject.SetActive(true);
            View.CatalistText.transform.parent.gameObject.SetActive(true);
            View.HerbsText.transform.parent.gameObject.SetActive(true);
            
            View.ScrapText.text = $"x{scrap.Amount}";
            View.CatalistText.text = $"x{catalist.Amount}";
            View.HerbsText.text = $"x{herbs.Amount}";
        }

        public void Hide()
        {
            View.ScrapText.transform.parent.gameObject.SetActive(false);
            View.CatalistText.transform.parent.gameObject.SetActive(false);
            View.HerbsText.transform.parent.gameObject.SetActive(false);
            
            View.ScrapText.text = $"0";
            View.CatalistText.text = $"0";
            View.HerbsText.text = $"0";
        }
    }
}