using UnityEngine;
using UnityEngine.UI;

public class PotionHudView : MonoBehaviour
{
    [Header("Potion amount`s")]
    public Text CrrPotionAmount;
    public Text PrevPotionAmount;
    public Text NextPotionAmount;

    [Space] 
    [Header("Potion icon`s")] 
    public Image CrrPotionIcon;
    public Image PrevPotionIcon;
    public Image NextPotionIcon;

    [Space] 
    [Header("Potion animator")] 
    public Animator PotionAnimator;
}
