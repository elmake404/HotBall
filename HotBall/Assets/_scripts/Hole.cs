using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] _meshRenderers;
    [SerializeField]
    private Transform _forvardWall, _beakWall;

    public void NewMaterial (Material material)
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _meshRenderers[i].material = material;
        }
    }
    public void SettingTheSizeOfTheWalls(Transform terrain,float depthTerrain)
    {
        Vector3 ScaleWall = _forvardWall.localScale;
        ScaleWall.z = (_forvardWall.position - terrain.position).z;
        _forvardWall.localScale = ScaleWall;

        ScaleWall = _beakWall.localScale;
        Vector3 pos = terrain.position;
        pos.z += depthTerrain;
        ScaleWall.z = (pos-_beakWall.position).z;
        _beakWall.localScale = ScaleWall;

    }    
}
