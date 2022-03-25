using UnityEngine;

public class CraftMenuView : MonoBehaviour
{
    // public Action<int> Craft;
    public GameObject slotPrefab;
    //
    // public void CraftPotion(int potionId)
    // {
    //     Craft(potionId);
    // }

    // void Awake()
    // {
    //     var recipes = ((Pot) GameObject.FindWithTag("Player").GetComponent<PlayerBase>().GetInteractable).Recepies;
    //     for (var index = 0; index < recipes.Count; index++)
    //     {
    //         Recipe recipe = recipes[index];
    //         var newSlot = Instantiate(slotPrefab, transform);
    //         var index1 = index;
    //         newSlot.transform.GetComponentsInChildren<Button>()[0].onClick.AddListener(() =>
    //             CraftPotion(index1));
    //     }
    // }

}
