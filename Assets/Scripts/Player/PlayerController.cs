using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public float Speed;
        public float SprintMultiplier;
        private Vector3 _moveDir;

        private Animator _animator;
        private PlayerBase _playerBase;
        
        [SerializeField] private bool _isAttacking;

        void Start()
        {
            _moveDir = Vector3.zero;
            _animator = GetComponent<Animator>();
            _playerBase = GetComponent<PlayerBase>();
        }

        void Update()
        {
            if (_playerBase.GetState.IsDead() || _playerBase._gui.StartScreenUi.IsGameStopped) return;
            
            StartAttack();
            Move();
        }

        private void Move()
        {
            if (_isAttacking) return;
        
            _moveDir = new Vector3(
                Input.GetAxisRaw("Horizontal"), 0,
                Input.GetAxisRaw("Vertical"));
            var moveSpeed = Speed;
        
            _moveDir = Quaternion.Euler(0, -90, 0) * _moveDir;

            if(_moveDir != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _animator.SetBool("isMoving", false);
                    _animator.SetBool("isRunning", true);
                    moveSpeed *= SprintMultiplier;
                }
                else
                {
                    _animator.SetBool("isMoving", true);   
                    _animator.SetBool("isRunning", false);
                }
            
                transform.position += _moveDir * moveSpeed * Time.deltaTime;
                transform.LookAt(transform.position + _moveDir);
            } else
            {
                _animator.SetBool("isMoving", false);
                _animator.SetBool("isRunning", false);
            }
        }

        public void LookToCursor()
        {
            var pos = (Camera.main.GetComponent<CameraController>().GetCursorPosition - transform.position)
                .normalized * AttackCenterOffset;
            transform.LookAt(transform.position + pos);
        }

        [Header("Attack")] 
        public float AttackCenterOffset;
        public float AttackVerticalPos;
        public float AttackHalfSize;
        public float ForcedAttackSlideDistance;

        private void StartAttack()
        {
            if(Input.GetMouseButtonDown(0) && !_isAttacking)
            {
                _isAttacking = true;
                _animator.SetTrigger("isAttackNormal");
            }
        }

        public void DealDamage()
        {
            var attackCenterPos = (Camera.main.GetComponent<CameraController>().GetCursorPosition - transform.position)
                .normalized * AttackCenterOffset;
            attackCenterPos.y = AttackVerticalPos;
          
            var colliders = Physics.OverlapBox(transform.position + attackCenterPos,
                new Vector3(AttackHalfSize, AttackHalfSize, AttackHalfSize));

            foreach (Collider col in colliders)
            {
                var damagable = col.GetComponent<IDamagable>();
                damagable?.GetDamage(100);
            }
        }

        public void FinishAttack()
        {
            if(_isAttacking)
                _isAttacking = false;
        }

        public void ForcedAttackSlide(float time)
        {
            transform.position += transform.forward * (ForcedAttackSlideDistance / time) * Time.deltaTime;
        }

        public void Kill()
        {
            _animator.SetTrigger("Dead");
        }

        public void Revive()
        {
            _playerBase.GetState.RestoreHp();
            _animator.SetTrigger("Revive");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

        
            if(Application.isPlaying)
            {
                var size = new Vector3(AttackHalfSize, AttackHalfSize, AttackHalfSize);
                var pos = (Camera.main.GetComponent<CameraController>().GetCursorPosition - transform.position)
                    .normalized * AttackCenterOffset;
                pos.y = AttackVerticalPos;
            
                Gizmos.DrawWireCube(transform.position + pos, size);
            }
        
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0f, 0f, ForcedAttackSlideDistance));
        }
    }
}