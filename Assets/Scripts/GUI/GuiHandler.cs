using Assets.Scripts.GUI.RecipeItemsHud;
using UnityEngine;

public class GuiHandler : MonoBehaviour
{
    public PotionHudViewController PotionHud;
    public InventoryHudViewController InventoryHud;
    public AreaTitleHudViewController AreaTitleHud;
    public DropInfoHudViewController DropInfoHud;
    public PlayerStatusHudViewController PlayerStatusHud;
    public DeadScreenUiViewController DeadScreenUi;
    public StartScreenUiViewController StartScreenUi;
    public StatusBarHudViewController StatusBarHud;
    public RecipeItemsHudViewController RecipeItemsHud;

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player"); 
        
        if (player != null)
        {
            if (StartScreenUi.IsGameStopped && Input.anyKeyDown)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                    Application.Quit();
                else
                    StartScreenUi.HideStartScreenUi();
            }
            else if (!StartScreenUi.IsGameStopped && Input.GetKeyDown(KeyCode.Escape))
                StartScreenUi.ShowStartScreenUi();
        }
    }
}
