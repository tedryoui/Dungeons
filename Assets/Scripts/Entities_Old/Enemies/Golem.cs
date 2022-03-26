using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.Entities_Old.Enemies;

public class Golem : Enemy, IDamagable
{
    [SerializeField] private GameObject HitParticle;
    public EnemyState EnemyState;
    
    void Start()
    {
        InitDefaults();
        EnemyState.Init(this);
    }

    void Update()
    {
        EnemyUpdate();
    }

    protected override IEnumerator Attack()
    {
        NavMeshAgent.isStopped = true;
        
        CrrAction = ChooseAction();
        var delay = Random.Range(0f, CrrAction.Delay);
        yield return new WaitForSeconds(delay);

        if (NavMeshAgent.remainingDistance >= AttackDistance)
        {
            NavMeshAgent.isStopped = false;
            Status = EnemyStatus.Chaise;
            CrrAction = null;
        } else
        {
            transform.rotation = Quaternion.LookRotation( Player.transform.position - transform.position);
            Animator.SetTrigger(CrrAction.ActionName);
        }
    }

    protected void CastCommonAttack()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position + transform.TransformVector(CrrAction.AttackOffset), CrrAction.AttackSizes);
        if(colliders.Any(x => x.CompareTag("Player")) && CrrAction != null)
        {
            Player.GetState.Damage(CrrAction.Damage);

            var particlePos = new Vector3(Player.transform.position.x, CrrAction.AttackOffset.y, Player.transform.position.z);
            Instantiate(HitParticle, particlePos, Quaternion.identity, null);
        }
    }

    protected void CastForcedAttack()
    {
        Collider[] colliders =
            Physics.OverlapSphere(transform.position + transform.TransformVector(CrrAction.AttackOffset), CrrAction.AttackSizes);
        if(colliders.Any(x => x.CompareTag("Player")) && CrrAction != null)
        {
            Player.GetState.Damage(CrrAction.Damage);

            var particlePos = new Vector3(Player.transform.position.x, CrrAction.AttackOffset.y, Player.transform.position.z);
            Instantiate(HitParticle, particlePos, Quaternion.identity, null);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, TriggerDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DefendDistance);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);
        
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(WaitingPosition.position, ChaiseDistance);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + transform.TransformVector(CrrAction.AttackOffset), CrrAction.AttackSizes);
    }

    public void GetDamage(float damage)
    {
        EnemyState.Damage(damage);
    }
}
