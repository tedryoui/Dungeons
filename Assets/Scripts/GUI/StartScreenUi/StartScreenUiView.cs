using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenUiView : MonoBehaviour
{
    public Animator StartScreenUiAnimator;
    public AudioSource AudioSource;

    public void StopTime() => Time.timeScale = 0f;
}
