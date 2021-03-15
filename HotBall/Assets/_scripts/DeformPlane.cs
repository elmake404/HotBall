using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformPlane : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private Vector3[] _verts;

    [SerializeField]
    private float _radius, _power;
    void Start()
    {
        _mesh = _meshFilter.mesh;
        _verts = _mesh.vertices;
    }

    public void DeformMesh(Vector3 positionToDeform )
    {
        positionToDeform = transform.InverseTransformPoint(positionToDeform);

        for (int i = 0; i < _verts.Length; i++)
        {
            float dist = (_verts[i] - positionToDeform).sqrMagnitude;
            if (dist<_radius)
            {
                _verts[i] -= Vector3.back * _power;
            }
            
        }
        _mesh.vertices = _verts;
    }
}
