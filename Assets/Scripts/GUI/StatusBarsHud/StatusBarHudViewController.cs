using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class StatusBarHudViewController
{
    public GameObject StatusBarPrefab;
    public Transform Parent;
    
    public StatusBarHudView CreateOne(Transform target, Vector3 offset = default)
    {
        var statusBar = Object.Instantiate(StatusBarPrefab, Parent);
        var statusBarView = statusBar.GetComponent<StatusBarHudView>();
        statusBarView.BindTo(target, offset);
        return statusBarView;
    }

    public void UpdateOne(StatusBarHudView hudView, IEntityState entityState)
    {
        hudView.FillArea.fillAmount = entityState.CrrHealth / entityState.MaxHealth;
    }

    public void RemoveOne(StatusBarHudView hudView)
    {
        Object.Destroy(hudView.gameObject);
    }
}
