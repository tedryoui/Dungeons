using System;
using UnityEngine;
using UnityEngine.UI;

public class DropInfo : MonoBehaviour
{
    [SerializeField] private Text _dropText;
    [SerializeField] private Image _dropImage;
    public ItemAmount Item;

    public void SetDefaults(ItemAmount item)
    {
        _dropText.text = $"{item.Item.Name} {item.Amount}x";
        _dropImage.sprite = item.Item.Icon;
        Item = item;
    }

    public void UpdateAmount(int amount)
    {
        Item.Amount += amount;
        _dropText.text = $"{Item.Item.Name} {Item.Amount}x";
    }
}