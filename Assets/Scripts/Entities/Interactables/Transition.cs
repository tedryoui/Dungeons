using Assets.Scripts.Entities.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.Entities.Interactables
{
    public class Transition : MonoBehaviour, IInteractable
    {
        [Inject] private PlayerBase _playerBase;
        [Inject] private GuiHandler _guiHandler;

        [SerializeField] private string _levelName;

        [SerializeField] private float _interactRange;

        public float InteractRange => _interactRange;
        public GameObject InteractGameObject => Resources.Load<GameObject>("Prefabs/QuestionMark");
        
        public Vector3 QuestionMarkOffset;
        private GameObject _questionMarkReference = null;

        public void Interact()
        {
            SceneManager.LoadScene(_levelName);
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
                transform);
        }

        private void Update()
        {
            if (IsInRange() && Input.GetKeyDown(KeyCode.F))
                Interact();
        }
    }
}