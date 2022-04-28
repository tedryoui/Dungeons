using System.Collections.Generic;
using Assets.Scripts.Item;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public List<ItemAmount> IN;
    public List<ItemAmount> OUT;

    
}
