using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarHudView : MonoBehaviour
{
    private Transform _target;
    private Vector3 _offset = Vector3.zero;

    public Image FillArea;

    public void BindTo(Transform target, Vector3 offset)
    {
        _target = target;
        _offset = offset;
    }

    void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(
            _target.transform.position + _offset);
    }

}
