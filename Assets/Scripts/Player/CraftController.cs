using System;
using UnityEngine;
using Zenject;

[Serializable]
public class CraftController
{
    [Inject] public PlayerBase _player;
    [Inject] public GuiHandler _guiHandler;
    
    public bool IsOpened { get; private set; }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!IsOpened)  Open();
            else Close();
        }
    }

    public void Close()
    {
        IsOpened = false;
        //_guiHandler.ToggleCraftMenu(IsOpened);
    }

    public void Open()
    {
        IsOpened = true;
        //_guiHandler.ToggleCraftMenu(IsOpened);
    }
}