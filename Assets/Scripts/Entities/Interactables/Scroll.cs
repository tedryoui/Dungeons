using System;
using Assets.Scripts.Entities.Player;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Assets.Scripts.Entities.Interactables
{
    public class Scroll : MonoBehaviour, IInteractable
    {
        [Inject] private PlayerBase _playerBase;
        [Inject] private GuiHandler _guiHandler;
        
        [SerializeField] private string date;
        [TextArea] 
        [SerializeField] private string message;
        [SerializeField] private Sprite signature;

        [SerializeField] private float _interactRange;

        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clip;

        public float InteractRange => _interactRange;
        public GameObject InteractGameObject => Resources.Load<GameObject>("Prefabs/QuestionMark");
        
        public Vector3 QuestionMarkOffset;
        private GameObject _questionMarkReference = null;

        private bool _isOpened = false;
        
        public void Interact()
        {
            if (_isOpened)
            {
                _guiHandler.ScrollScreen.Hide();
                _audioSource.PlayOneShot(_clip);
                _isOpened = false;
            }
            else
            {
                _guiHandler.ScrollScreen.Show(signature, message, date);
                _audioSource.PlayOneShot(_clip);
                _isOpened = true;
            }
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
        }

        private void DisplayPlayerIn()
        {
            _questionMarkReference = Instantiate(InteractGameObject,
                transform.position + QuestionMarkOffset,
                Quaternion.identity,
                null);
        }

        private void Update()
        {
            if (IsInRange() && Input.GetKeyDown(KeyCode.R))
            {
                Interact();
            }
        }
    }
}