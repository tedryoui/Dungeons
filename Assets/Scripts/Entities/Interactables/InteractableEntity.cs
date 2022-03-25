using UnityEngine;

public class InteractableEntity : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) 
            other.gameObject.GetComponent<PlayerBase>().SetInteractable(this);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player")) 
            other.gameObject.GetComponent<PlayerBase>().RemoveInteractable(this);
    }
}