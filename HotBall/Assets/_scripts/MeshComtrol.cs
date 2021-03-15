using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshComtrol : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _mesh;

    void Start()
    {
        Debug.Log(_mesh.mesh.vertices.Length);
        //for (int i = 0; i < _mesh.mesh.vertices.Length; i++)
        //{
        //    Debug.Log(_mesh.mesh.vertices[i]);

        //}

    }

    // Update is called once per frame
    void Update()
    {

    }
}
