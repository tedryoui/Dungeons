using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    public class EnemySpot : MonoBehaviour
    {
        public float PlayerInRadius;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            
            Gizmos.DrawWireSphere(transform.position, PlayerInRadius);
        }
    }
}