using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = System.Random;
using UnityRandom = UnityEngine.Random;

namespace Assets.Scripts.Entities.Enemies
{
    [Serializable]
    public class EnemyMoveController : EntityController
    {
        protected enum EnemyControllerStatus {Wait, Chaise, Return, Prepare, Away, Attack }

        private EnemyBase _enemyBase;
        private NavMeshAgent NavMeshAgent => _enemyBase.NavMeshAgent;
        private Animator Animator => _enemyBase.Animator;
        private Vector3 PlayerPosition => _enemyBase.PlayerBase.transform.position;

        [Header("Area Triggers")] 
        [SerializeField] protected float TriggerDistance;
        [SerializeField] protected float DefendDistance;
        [SerializeField] protected float AttackDistance;
        
        private Vector3 CrrTargetPosition { get; set; }
        
        [Header("Actions")]
        [SerializeField] protected List<EnemyAction> Actions;
        
        [Space]
        [SerializeField] protected EnemyAction CrrAction;
        [SerializeField] protected EnemyControllerStatus Status;

        [Space] [SerializeField] protected GameObject Boom;
        protected Vector3 SpawnPoint;
        protected override void Initialize<T1, T2>(EntityBase<T1, T2> _entityBase)
        {
            _enemyBase = _entityBase as EnemyBase;
        
            NavMeshAgent.stoppingDistance = DefendDistance;
            NavMeshAgent.speed = _enemyBase.GetState.Speed;
        }

        protected override void Update()
        {
            if (_enemyBase.GetState.IsDead) return;

            Move();
        }

        protected void Move()
        {
            switch (Status)
            {
                case EnemyControllerStatus.Wait:
                    Wait();
                    break;
                case EnemyControllerStatus.Return:
                    Return();
                    break;
                case EnemyControllerStatus.Chaise:
                    Chaise();
                    break;
                case EnemyControllerStatus.Prepare:
                    Prepare();
                    break;
                case EnemyControllerStatus.Away:
                    Away();
                    break;
            }
        }
        
        protected void Return()
        {
            NavMeshAgent.isStopped = false;
            Animator.SetBool("isMoving", true);
            
            SetDestination(SpawnPoint);
            Status = EnemyControllerStatus.Return;
            _enemyBase.GetState.IsVulnerable = false;

            if (IsReachedDestination())
            {
                _enemyBase.GetState.IsVulnerable = true;
                Wait();
            }
        }

        protected void Wait()
        {
            NavMeshAgent.isStopped = true;
            Animator.SetBool("isMoving", false);
            
            Status = EnemyControllerStatus.Wait;

            //Make coroutine to wait and go
            if (Vector3.Distance(PlayerPosition, _enemyBase.transform.position) <= TriggerDistance)
                Chaise();
        }

        protected void Chaise()
        {
            if (ReturnIfPlayerUndamagable()) return;
            
            SetDestination(PlayerPosition);
            Status = EnemyControllerStatus.Chaise;

            if (NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance)
            {
                NavMeshAgent.isStopped = false;
                Animator.SetBool("isMoving", true);
            }
            else
                Prepare();
            
            //Make coroutine to wait, chech for position and then go
            if(NavMeshAgent.remainingDistance > TriggerDistance) 
                Return();
            // else if (Vector3.Distance(PlayerPosition, SpawnPoint) > 10f)
            //     Return();
        }

        protected void Prepare()
        {
            if (ReturnIfPlayerUndamagable()) return;
            
            Status = EnemyControllerStatus.Prepare;
            SetDestination(PlayerPosition);

            if (NavMeshAgent.remainingDistance >= NavMeshAgent.stoppingDistance &&
                NavMeshAgent.remainingDistance <= AttackDistance)
            {
                NavMeshAgent.isStopped = true;
                Animator.SetBool("isMoving", false);

                _enemyBase.StartCoroutine(Attack());
                Status = EnemyControllerStatus.Attack;
            }
            else if (NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance)
                Away();
            else 
                Chaise();
        }

        protected void Away()
        {
            Status = EnemyControllerStatus.Away;
            SetDestination(PlayerPosition);
            
            //Make smooth rotating
            _enemyBase.transform.rotation = 
                Quaternion.LookRotation( PlayerPosition - _enemyBase.transform.position);

            if (NavMeshAgent.remainingDistance > NavMeshAgent.stoppingDistance)
            {
                Animator.SetBool("isFleeing", false);
                Prepare();
            }
            else
            {
                _enemyBase.transform.position += (CrrTargetPosition - _enemyBase.transform.position).normalized
                                      * -1 * (_enemyBase.GetState.Speed / 2) * Time.deltaTime;
                Animator.SetBool("isFleeing", true);
                Animator.SetBool("isMoving", false);
            }
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

        protected IEnumerator Attack()
        {
            NavMeshAgent.isStopped = true;
        
            CrrAction = ChooseAction();
            var delay = UnityRandom.Range(0f, CrrAction.Delay);
            yield return new WaitForSeconds(delay);

            if (NavMeshAgent.remainingDistance >= AttackDistance)
            {
                NavMeshAgent.isStopped = false;
                Status = EnemyControllerStatus.Chaise;
                CrrAction = null;
            } else
            {
                _enemyBase.transform.rotation = 
                    Quaternion.LookRotation( PlayerPosition - _enemyBase.transform.position);
                Animator.SetTrigger(CrrAction.ActionName);
            }
        }
        
        public void OverlapAttackAreaByCrrAction()
        {
            Collider[] colliders =
                Physics.OverlapSphere(
                        _enemyBase.transform.position + 
                            _enemyBase.transform.TransformVector(CrrAction.AttackOffset), 
                            CrrAction.AttackSizes);
            
            if(colliders.Any(x => x.CompareTag("Player")) && CrrAction != null)
            {
                if(_enemyBase.PlayerBase.GetState.Injure(CrrAction.Damage))
                {
                    var particlePos = new Vector3(PlayerPosition.x, CrrAction.AttackOffset.y, PlayerPosition.z);
                    Object.Instantiate(CrrAction.HitParticle, particlePos, Quaternion.identity, null);
                }
            }
        }
        
        public void FinishAttack()
        {
            Status = EnemyControllerStatus.Prepare;
            CrrAction = null;
        }
        public IEnumerator Fall()
        {
            GameObject.Instantiate(Boom, _enemyBase.transform.position, Quaternion.identity, _enemyBase.transform);
            
            yield return new WaitForSeconds(4);
            _enemyBase.OnDestroyAction();
            
            _enemyBase.Animator.SetTrigger("Reset");
        }
        protected bool IsReachedDestination()
        {
            if (NavMeshAgent.hasPath)
                return NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance;
            else return false;
        }
        private bool ReturnIfPlayerUndamagable()
        {
            if (_enemyBase.PlayerBase.GetState.IsDead ||
                !_enemyBase.PlayerBase.GetState.IsVulnerable)
            {
                Status = EnemyControllerStatus.Return;
                return true;
            }

            return false;
        }
        protected void SetDestination(Vector3 target)
        {
            CrrTargetPosition = target;
            _enemyBase.NavMeshAgent.SetDestination(target);
        }
        public void Stop()
        {
            CrrAction = null;
            Status = EnemyControllerStatus.Wait;

            _enemyBase.Animator.enabled = false;
            _enemyBase.NavMeshAgent.enabled = false;
        }

        public void Start(Vector3 spawnPoint)
        {
            SpawnPoint = spawnPoint;
            Status = EnemyControllerStatus.Return;
            
            _enemyBase.Animator.enabled = true;
            _enemyBase.NavMeshAgent.enabled = true;
        }

        public void DrawGizmos(Transform parent)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(parent.position, DefendDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(parent.position, AttackDistance);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(parent.position, TriggerDistance);
            
            if(CrrAction != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(parent.position + parent.TransformVector(CrrAction.AttackOffset),
                    CrrAction.AttackSizes);
            }
        }
    }
}