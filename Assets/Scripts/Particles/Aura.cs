using UnityEngine;

public class Aura : MonoBehaviour
{
    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        transform.position = _target.position;
    }
}
