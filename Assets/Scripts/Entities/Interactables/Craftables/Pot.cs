using Assets.Scripts.Entities.Player;
using Assets.Scripts.Item;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Entities.Interactables.Craftables
{
    public class Pot : MonoBehaviour, ICraftable
    {
        [Inject] private PlayerBase _playerBase;
        [SerializeField] private Recipe _potionRecipe;
        [SerializeField] private float _interactRange;
        public Vector3 QuestionMarkOffset;
        public Recipe GetRecipe => _potionRecipe;

        public float InteractRange => _interactRange;
        public GameObject InteractGameObject => Resources.Load<GameObject>("Prefabs/QuestionMark");

        private GameObject _questionMarkReference = null;
        public void Interact()
        {
            if (CanCraft(_playerBase.Inventory))
                Craft(_playerBase.Inventory);
        }

        public bool IsInRange()
        {
            if(Vector3.Distance(_playerBase.transform.position, transform.position) < InteractRange)
            {
                if (!_questionMarkReference)
                   DisplayPlayerIn();
                
                return true;
            }

            if (_questionMarkReference)
                DisplayPlayerOut();

            return false;
        }

        private void DisplayPlayerOut()
        {
            GameObject.Destroy(_questionMarkReference);
            
            _playerBase.GuiHandler.RecipeItemsHud.Hide();
        }

        private void DisplayPlayerIn()
        {
            _questionMarkReference = Instantiate(InteractGameObject,
                transform.position + QuestionMarkOffset,
                Quaternion.identity,
                transform);

            _playerBase.GuiHandler.RecipeItemsHud.Show(_potionRecipe.IN);
        }

        public bool CanCraft(IItemContainer container)
        {
            foreach (ItemAmount itemAmount in GetRecipe.IN)
                if (!container.CheckForItem(itemAmount.Item, itemAmount.Amount)) return false;
            return true;
        }

        public void Craft(IItemContainer container)
        {
            GetRecipe.IN.ForEach((value) => 
                container.RemoveItem(value.Item, value.Amount));
            
            GetRecipe.OUT.ForEach((value) => 
                container.AddItem(value.Item, value.Amount));
        }

        void Update()
        {
            if(IsInRange() && Input.GetKeyDown(KeyCode.C))
                Interact();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, InteractRange);
        }
    }
}