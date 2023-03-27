using System;
using System.Linq;
using Assets.Scripts.Item;
using UnityEngine;

[Serializable]
public class PotionHudViewController
{
    public PotionHudView View;
    [SerializeField] private AudioClip _changedClip;
    
    private Color32 _transparentColor = new Color32(0, 0, 0, 0);
    private Color32 _defaultColor = new Color32(255, 255, 255, 255);
    private Color32 _darkenColor = new Color32(170, 170, 170, 255);
    
    public void Update(IItemContainer container, int crrPointer)
    {
        var potions = container.Items.
            Where(x => x.Item is Potion).
            ToList();
        int count = potions.Count;
        
        View.AudioSource.PlayOneShot(_changedClip);

        if (count == 0)
        {
            SetIcons();
            SetAmounts();
            return;
        }
        
        int prev = (crrPointer == 0) ? count - 1: crrPointer - 1, 
            next = (crrPointer == count - 1) ? 0 : crrPointer + 1;

        ItemAmount  crrItem = potions[crrPointer],
                    prevItem = (crrPointer == prev) ? crrItem : potions[prev],
                    nextItem = (crrPointer == next) ? crrItem : potions[next];

        if (count == 1)
        {
            SetIcons(crrItem.Item.Icon);
            SetAmounts(crrItem.Amount);
        }
        else
        {
            SetIcons(crrItem.Item.Icon, prevItem.Item.Icon, nextItem.Item.Icon);
            SetAmounts(crrItem.Amount, prevItem.Amount, nextItem.Amount);
        }
    }

    public void PulseCrr()
    {
        View.PotionAnimator.SetTrigger("isPulse");
    }

    private void SetIcons(Sprite crr = null, Sprite prev = null, Sprite next = null)
    {
        View.CrrPotionIcon.sprite = crr;
        View.CrrPotionIcon.color = (crr) ? _defaultColor : _transparentColor;
        View.PrevPotionIcon.sprite = prev;
        View.PrevPotionIcon.color = (prev) ? _darkenColor : _transparentColor;
        View.NextPotionIcon.sprite = next;
        View.NextPotionIcon.color = (next) ? _darkenColor : _transparentColor;
    }

    private void SetAmounts(int crr = 0, int prev = 0, int next = 0)
    {
        View.CrrPotionAmount.text = $"x{crr.ToString()}";
        View.PrevPotionAmount.text = $"x{prev.ToString()}";
        View.NextPotionAmount.text = $"x{next.ToString()}";
    }
}
