using UnityEngine;
using Zenject;

public class Area : MonoBehaviour
{
    [Inject] private GuiHandler _gui;
    public string Title;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _gui.AreaTitleHud.ShowAndHide(Title);
    }
}
