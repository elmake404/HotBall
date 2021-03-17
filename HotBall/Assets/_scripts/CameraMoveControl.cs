using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveControl : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    private Vector3 _ofset;

    void Start()
    {
        _ofset = transform.position - _target.position;
    }

    void LateUpdate()
    {
        transform.position = _target.position + _ofset;
    }
}
