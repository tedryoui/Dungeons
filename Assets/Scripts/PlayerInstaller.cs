using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public GameObject PlayerPrefab;
    public Transform location;
    
    [Space]
    public GameObject GuiPrefab;
    
    public override void InstallBindings()
    {
        InstallGui();
        InstallPlayer();
    }

    private void InstallPlayer()
    {
        var player = Container.InstantiatePrefabForComponent<PlayerBase>(PlayerPrefab, 
            location.position, 
            Quaternion.identity, 
            null);

        Container.Bind<PlayerBase>().FromInstance(player).AsSingle();
        player.GetComponent<PlayerBase>().enabled = true;
        
        Destroy(location.gameObject);
    }
    
    private void InstallGui()
    {
        var inventoryHudView = Container.InstantiatePrefabForComponent<GuiHandler>(
            GuiPrefab, Vector3.zero, Quaternion.identity, null);
        
        Container.Bind<GuiHandler>().FromInstance(inventoryHudView).AsSingle();
    }
}
