using System;
using Assets.Scripts.Entities.Player;
using Zenject;

[Serializable]
public class CraftMenuViewController
{
    [Inject] private PlayerBase _playerBase;
    
    public CraftMenuView CraftMenuView;
    
    private bool isOpen = false;
    // public void Bind()
    // {
    //     CraftMenuView.Craft += CraftPotion;
    // }

    // private void CraftPotion(int id)
    // {
    //     ((Pot)_playerBase.GetInteractable).Recepies[id].Craft(_player.GetItemContainer, _player.GetPotionContainer);
    // }

    public bool Toggle(bool mode)
    {
        isOpen = mode;
        CraftMenuView.gameObject.SetActive(mode);
        return mode;
    }
}
