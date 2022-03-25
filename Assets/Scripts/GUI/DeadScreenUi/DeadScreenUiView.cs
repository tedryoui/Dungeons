using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DeadScreenUiView : MonoBehaviour
{
    public Animator DeadScreenUiAnimator;
    public Button Restart;

    public void Revive()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerBase>()?.Revive();
    }
}