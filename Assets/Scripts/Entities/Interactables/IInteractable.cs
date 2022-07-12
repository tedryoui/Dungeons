using UnityEngine;

namespace Assets.Scripts.Entities.Interactables
{
    public interface IInteractable
    {
        public float InteractRange { get; }
        public GameObject InteractGameObject { get; }
        public void Interact();
        public bool IsInRange();
    }
}