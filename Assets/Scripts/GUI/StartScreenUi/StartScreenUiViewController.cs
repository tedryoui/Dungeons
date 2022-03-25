using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StartScreenUiViewController
{
    public StartScreenUiView View;
    public bool IsGameStopped = true;

    public void ShowStartScreenUi()
    {
        View.StartScreenUiAnimator.SetTrigger("Show");
        IsGameStopped = true;
    }
    
    public void HideStartScreenUi()
    {
        View.StartScreenUiAnimator.SetTrigger("Hide");
        Time.timeScale = 1f;
        IsGameStopped = false;
    }
}
