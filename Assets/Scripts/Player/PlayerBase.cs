using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Zenject;

public class PlayerBase : MonoBehaviour
{
    [Inject] public GuiHandler _gui;
    
    private PlayerController _controller;
    [SerializeField] private CraftController _craftController;
    [SerializeField] private Inventory _inventory = new Inventory();
    [SerializeField] private PlayerState _state;
    private List<InteractableEntity> _interactable = new List<InteractableEntity>();
    public PlayerState GetState => _state;
    public IItemContainer GetItemContainer => (IItemContainer)_inventory;
    public InteractableEntity GetInteractable => _interactable[0];

    private Rigidbody _rb;

    void Start()
    {
        _controller = gameObject.GetComponent<PlayerController>();
        _inventory.Init(_gui, this);
        _state.Init(transform, _gui);
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _rb.velocity = Vector3.zero;

        if (_state.IsDead() || _gui.StartScreenUi.IsGameStopped) return;
        if(_interactable.Count > 0) UpdateInteractableList();
        
        _inventory.Update();
    }

    public void SetInteractable(InteractableEntity _other)
    {
        if(!_interactable.Contains(_other))
        {
            _interactable.Add(_other);   
        }
    }

    public void RemoveInteractable(InteractableEntity _other)
    {
        if (_interactable.Contains(_other))
        {
            _interactable.Remove(_other);
        }
    }

    private void UpdateInteractableList()
    {
        var prevInt = _interactable[0];
        _interactable = _interactable.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).ToList();

        if (prevInt != _interactable[0]) CloseAllInteractable();
        
        _craftController.Update();
    }

    private void CloseAllInteractable()
    {
        _craftController.Close();
    }

    public void Kill()
    {
        _controller.Kill();
        _gui.DeadScreenUi.ShowDeadScreen();
        
        Camera.main.GetComponent<CameraController>().UnbindCursorToWorld();
    }

    public void Revive()
    {
        _controller.Revive();
        _gui.DeadScreenUi.HideDeadScreen();
        
        Camera.main.GetComponent<CameraController>().BindCursorToWorld();
    }
}
