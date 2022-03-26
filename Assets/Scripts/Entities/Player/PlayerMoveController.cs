using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    [Serializable]
    public class PlayerMoveController : EntityController
    {
        enum PlayerControllerStatus { Stay, Walk, Run, Attack }

        public float Speed;
        public float SprintMultiplier;

        private Vector3 _moveDir;
        private PlayerBase _playerBase;

        [SerializeField] private PlayerControllerStatus _controllerStatus;

        protected override void Initialize<T1, T2>(EntityBase<T1, T2> _entityBase)
        {
            _moveDir = Vector3.zero;

            _playerBase = _entityBase as PlayerBase;
        }

        protected override void Update()
        {
            TryAttack();
            if (_controllerStatus != PlayerControllerStatus.Attack)
            {
                DefineMoveDir();
                Move();
            }
        }

        private void DefineMoveDir()
        {
            _moveDir = new Vector3(
                Input.GetAxisRaw("Horizontal"), 0,
                Input.GetAxisRaw("Vertical"));

            _moveDir = Quaternion.Euler(0, -90, 0) * _moveDir;
        }
        
        private void UpdateControllerStatus()
        {
            if (_moveDir == Vector3.zero) _controllerStatus = PlayerControllerStatus.Stay;
            else if (Input.GetKey(KeyCode.LeftShift)) _controllerStatus = PlayerControllerStatus.Run;
            else _controllerStatus = PlayerControllerStatus.Walk;
        }

        private void Move()
        {
            UpdateControllerStatus();

            switch (_controllerStatus)
            {
                case PlayerControllerStatus.Stay:
                    Stay();
                    break;
                case PlayerControllerStatus.Walk:
                    Walk();
                    break;
                case PlayerControllerStatus.Run:
                    Run();
                    break;
            }
        }

        private void Run()
        {
            _playerBase.Animator.SetBool("isMoving", false);
            _playerBase.Animator.SetBool("isRunning", true);

            //Translating with a [Speed * SpeedMultiplier] by [moveDir]
            _playerBase.transform.position += _moveDir * (Speed * SprintMultiplier) * Time.deltaTime;

            //Rotating to the [moveDir]
            _playerBase.transform.LookAt(_playerBase.transform.position + _moveDir);
        }

        private void Walk()
        {
            _playerBase.Animator.SetBool("isMoving", true);
            _playerBase.Animator.SetBool("isRunning", false);

            //Translating with a [Speed] by [moveDir]
            _playerBase.transform.position += _moveDir * Speed * Time.deltaTime;

            //Rotating to the [moveDir]
            _playerBase.transform.LookAt(_playerBase.transform.position + _moveDir);
        }

        private void Stay()
        {
            _playerBase.Animator.SetBool("isMoving", false);
            _playerBase.Animator.SetBool("isRunning", false);
        }

        [Header("Attack")] 
        public Vector3 AttackOffset;
        public float AttackSizes;
        public float ForcedAttackSlideDistance;

        public void LookToCursor()
        {
            var pos = (Camera.main.GetComponent<CameraController>().GetCursorPosition - _playerBase.transform.position);
            _playerBase.transform.LookAt(_playerBase.transform.position + pos);
        }

        private void TryAttack()
        {
            if (Input.GetMouseButtonDown(0) && _controllerStatus != PlayerControllerStatus.Attack)
            {
                _playerBase.Animator.SetTrigger("isAttackNormal");
                _controllerStatus = PlayerControllerStatus.Attack;
            }
        }

        public void DealDamage()
        {
            var colliders = Physics.OverlapSphere(
                _playerBase.transform.position + _playerBase.transform.TransformVector(AttackOffset),
                AttackSizes);

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Player")) return;
                
                var damagable = col.GetComponent<IDamagable>();
                damagable?.GetDamage(5);
            }
        }

        public void FinishAttack()
        {
            if (_controllerStatus == PlayerControllerStatus.Attack)
                _controllerStatus = PlayerControllerStatus.Stay;
        }

        public void ForcedAttackSlide(float time)
        {
            _playerBase.transform.position +=
                _playerBase.transform.forward * (ForcedAttackSlideDistance / time) * Time.deltaTime;
        }
    }
}