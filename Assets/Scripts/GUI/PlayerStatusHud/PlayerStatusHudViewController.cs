using System;
using Assets.Scripts.Entities.Player;

[Serializable]
public class PlayerStatusHudViewController
{
    public PlayerStatusHudView View;

    public void UpdateHealthBar(PlayerState state)
    {
        View.HealthText.text = $"{state.CrrHealth}/{state.MaxHealth}";
        View.HealthFiller.fillAmount = state.CrrHealth / state.MaxHealth;
    }
}
