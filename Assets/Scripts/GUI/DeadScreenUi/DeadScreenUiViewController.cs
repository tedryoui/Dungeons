using System;

[Serializable]
public class DeadScreenUiViewController
{
    public DeadScreenUiView View;

    public void ShowDeadScreen()
    {
        View.DeadScreenUiAnimator.SetTrigger("Show");
    }

    public void HideDeadScreen()
    {
        View.DeadScreenUiAnimator.SetTrigger("Hide");
    }
}