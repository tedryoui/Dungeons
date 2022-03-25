using System;

[Serializable]
public class AreaTitleHudViewController
{
    public AreaTitleHudView View;

    public void ShowAndHide(string title)
    {
        View.AreaTitle.text = title.ToUpperInvariant();
        View.AreaAnimator.SetTrigger("ShowAndHide");
    }
}
