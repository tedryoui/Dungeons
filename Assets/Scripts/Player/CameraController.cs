using System;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [Inject] private PlayerBase _target;
    public Transform Target => _target.transform;
    
    public Vector3 Offset;
    public float SmoothTime = 0.3f;
 
    private Vector3 velocity = Vector3.zero;
 
    private Camera GetCamera => this.gameObject.GetComponent<Camera>();
    
    private GameObject _cursor;

    private bool _isCustomCursor = true;
    public GameObject CursorPrefab;
    public Vector3 GetCursorPosition => _cursor.transform.position;
    void Awake()
    {
        _cursor = GameObject.Instantiate(CursorPrefab);

        Cursor.visible = false;
    }
    
    void Start()
    {
        Offset = transform.position - Target.position;

    }

    void Update()
    {
        if(_isCustomCursor) PlaceCursor();
        Follow();
    }

    void Follow()
    {
        Vector3 targetPosition = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
    }

    void PlaceCursor()
    {
        try
        {
            Vector3 pos = CalculatePos();
            
            _cursor.SetActive(true);
            _cursor.transform.position = new Vector3(pos.x, 0.2f, pos.z);
        }
        catch
        {
            _cursor.SetActive(false);
        }
    }

    private Vector3 CalculatePos()
    {
        Ray ray = GetCamera.ScreenPointToRay (Input.mousePosition);
        Plane groundPlane = new Plane (Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out var rayDistance))
            return ray.GetPoint(rayDistance);
        else throw new Exception();
    }

    public void BindCursorToWorld()
    {
        _isCustomCursor = true;
        Cursor.visible = false;
        _cursor.SetActive(true);
    }

    public void UnbindCursorToWorld()
    {
        _isCustomCursor = false;
        Cursor.visible = true;
        _cursor.SetActive(false);
    }
}
