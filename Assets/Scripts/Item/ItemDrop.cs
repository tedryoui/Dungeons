using System;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [Serializable]
    public class ItemDrop : ItemAmount
    {
        [Range(0, 1)] public float Chance;
    }
}