using UnityEngine;
using Zenject;

namespace Assets.Scripts.Entities.Player
{
    public class PlayerBase : EntityBase<PlayerMoveController, PlayerState>, IDamagable
    {
        [Inject] public GuiHandler GuiHandler;

        //Components
        public Animator Animator { get; private set; }
        
        //Controllers
        [SerializeField] private PlayerInventory _inventory;
        public PlayerInventory Inventory => _inventory;
        
        protected override void EntityStart()
        {
            Inventory.SafeInitialize(ref OnUpdateAction, this);
            
            Animator = GetComponent<Animator>();
        }

        protected override void EntityUpdate()
        {
            
        }

        public bool GetDamage(float damage) => StateController.Injure(damage);
        public void DealDamage() => MoveController.DealDamage();
        
        void OnDrawGizmosSelected()
        {
            
            //Attack sphere
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(
            transform.position + transform.TransformVector(MoveController.AttackOffset),
                MoveController.AttackSizes);
        }

        
    }    
}