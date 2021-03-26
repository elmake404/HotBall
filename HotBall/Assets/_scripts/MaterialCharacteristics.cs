using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCharacteristics : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private float _speedPenalty; public float SpeedPenalty { get { return _speedPenalty; } }
    [SerializeField]
    private bool _isDenseMetal;
    [SerializeField]
    private Hole _hole;
    [SerializeField]
    private DestructibleTerrain _destructibleTerrain;

    public void HoleCreation(Vector3 posCreatin)
    {
        posCreatin.y = transform.position.y + _destructibleTerrain.ResolutionY * _destructibleTerrain.BlockSize;
        Hole hole = Instantiate(_hole, posCreatin, _hole.transform.rotation);
        hole.NewMaterial(_destructibleTerrain.MaterialTerrail);

        Vector3 newScale = hole.transform.localScale;
        newScale.y = _destructibleTerrain.ResolutionY *_destructibleTerrain.BlockSize;
        hole.transform.localScale = newScale;
        hole.SettingTheSizeOfTheWalls(_destructibleTerrain.transform,_destructibleTerrain.Depth);
    }
    public bool IsDenseMetal() => _isDenseMetal;
}
