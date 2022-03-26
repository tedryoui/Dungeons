using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entities.Player;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = System.Random;

public abstract class Enemy : MonoBehaviour
{
    public enum EnemyStatus {Wait, Chaise, Return, Prepare, Away, Attack, Hitted }
    
    [Inject] protected PlayerBase Player;
    protected NavMeshAgent NavMeshAgent;
    protected Animator Animator;
    private Rigidbody Rigidbody;
    
    [Header("Area Triggers")] 
    [SerializeField] protected float TriggerDistance;
    [SerializeField] protected float DefendDistance;
    [SerializeField] protected float AttackDistance;
    [SerializeField] protected Transform WaitingPosition;
    [SerializeField] protected float ChaiseDistance;

    private Vector3 _crrTargetPosition;
    public Vector3 CrrTargetPosition
    {
        get
        {
            return _crrTargetPosition;
        }
        set
        {
            _crrTargetPosition = value;
            NavMeshAgent.SetDestination(value);
        }
    }

    [Header("Enemy stats")] 
    public float Speed;

    public bool isAlive = true;

    public List<EnemyAction> Actions;
    [SerializeField] protected EnemyAction CrrAction;

    public EnemyStatus Status;

    protected void EnemyUpdate()
    {
        if (!isAlive) return;
        
        Rigidbody.velocity = Vector3.zero;
        
        switch (Status)
        {
            case EnemyStatus.Wait:
                Wait();
                break;
            case EnemyStatus.Return:
                Return();
                break;
            case EnemyStatus.Chaise:
                Chaise();
                break;
            case EnemyStatus.Prepare:
                Prepare();
                break;
            case EnemyStatus.Away:
                Away();
                break;
        }
    }

    protected void InitDefaults()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Return();
        
        NavMeshAgent.stoppingDistance = DefendDistance;
        NavMeshAgent.speed = Speed;
    }
    
    protected void Return()
    {
        NavMeshAgent.isStopped = false;
        Animator.SetBool("isMoving", true);
        
        CrrTargetPosition = WaitingPosition.position;
        Status = EnemyStatus.Return;
        

        if (IsReachedDestination())
            Wait();
    }

    protected void Wait()
    {
        NavMeshAgent.isStopped = true;
        Animator.SetBool("isMoving", false);
        
        Status = EnemyStatus.Wait;

        //Make coroutine to wait and go
        if (Vector3.Distance(Player.transform.position, transform.position) <= TriggerDistance)
            Chaise();
    }

    protected void Chaise()
    {
        if (Player.GetState.IsDead)
        {
            Status = EnemyStatus.Return;
            return;
        }
        
        CrrTargetPosition = Player.transform.position;
        Status = EnemyStatus.Chaise;

        if (NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance)
        {
            NavMeshAgent.isStopped = false;
            Animator.SetBool("isMoving", true);
        }
        else
        {
            Prepare();
        }
        
        //Make coroutine to wait, chech for position and then go
        if(NavMeshAgent.remainingDistance > TriggerDistance) 
            Return();
        else if (Vector3.Distance(Player.transform.position, WaitingPosition.position) > ChaiseDistance)
            Return();
    }

    protected void Prepare()
    {
        if (Player.GetState.IsDead)
        {
            Status = EnemyStatus.Return;
            return;
        }
        
        Status = EnemyStatus.Prepare;
        CrrTargetPosition = Player.transform.position;

        if (NavMeshAgent.remainingDistance >= NavMeshAgent.stoppingDistance &&
            NavMeshAgent.remainingDistance <= AttackDistance)
        {
            NavMeshAgent.isStopped = true;
            Animator.SetBool("isMoving", false);

            StartCoroutine(Attack());
            Status = EnemyStatus.Attack;
        }
        else if (NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance)
            Away();
        else 
            Chaise();
    }

    public void FinishAttack()
    {
        Status = EnemyStatus.Prepare;
        CrrAction = null;
    }

    protected EnemyAction ChooseAction()
    {
        int offset = 0;
        var rangedItems = Actions
            .OrderBy(x => x.Chance)
            .Select(x => (x, RangeTo: offset += (int)(x.Chance * 100)))
            .ToArray();

        int randomNumber = new Random().Next(Actions.Sum(x => (int)(x.Chance * 100))) + 1;
        return rangedItems.First(x => randomNumber <= x.RangeTo).x;
    }

    protected abstract IEnumerator Attack();

    protected void Away()
    {
        Status = EnemyStatus.Away;
        CrrTargetPosition = Player.transform.position;
        
        transform.rotation = Quaternion.LookRotation( Player.transform.position - transform.position);

        if (NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance)
        {
            Animator.SetBool("isFleeing", false);
            Prepare();
        }
        else
        {
            transform.position += (CrrTargetPosition - transform.position).normalized
                * -1 * (Speed / 2) * Time.deltaTime;
            Animator.SetBool("isFleeing", true);
            Animator.SetBool("isMoving", false);
        }
    }

    protected bool IsReachedDestination() => NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance;

    protected void FallDown()
    {
        isAlive = false;
        StartCoroutine(Fall());
    }

    protected IEnumerator Fall()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.GetComponent<NavMeshAgent>());
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
    
    public void Kill()
    {
        isAlive = false;
        Animator.SetTrigger("Dead");
    }
}