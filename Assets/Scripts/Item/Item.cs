using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu]
    public class Item : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public int ItemID;
    }
}