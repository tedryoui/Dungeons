using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StartScreenUiViewController
{
    public StartScreenUiView View;
    public bool IsGameStopped = true;

    [SerializeField] private AudioClip _openClosedClip;

    public void ShowStartScreenUi()
    {
        View.StartScreenUiAnimator.SetTrigger("Show");
        View.AudioSource.PlayOneShot(_openClosedClip);
        IsGameStopped = true;
    }
    
    public void HideStartScreenUi()
    {
        View.StartScreenUiAnimator.SetTrigger("Hide");        
        View.AudioSource.PlayOneShot(_openClosedClip);
        Time.timeScale = 1f;
        IsGameStopped = false;
    }
}
