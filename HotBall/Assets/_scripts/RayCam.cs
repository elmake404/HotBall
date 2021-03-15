using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCam : MonoBehaviour
{
    private Ray _ray;
    private RaycastHit _raycastHit;

    private Camera _cam;
    private void Start()
    {
        _cam = Camera.main;

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            DeformMesh();
        }
    }
    private void DeformMesh()
    {
        _ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray,out _raycastHit))
        {
            DeformPlane deformPlane = _raycastHit.transform.GetComponent<DeformPlane>();
            deformPlane.DeformMesh(_raycastHit.point);
        }
    }
}
