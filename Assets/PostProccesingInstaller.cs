using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PostProccesingInstaller : MonoInstaller
{
    public GameObject PPPrefab;
    
    public override void InstallBindings()
    {
        InstallPostProccesingUnit();        
    }

    public void InstallPostProccesingUnit()
    {
        
    }
}
