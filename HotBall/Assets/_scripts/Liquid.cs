using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    [SerializeField]
    [Range(-100, 100)]
    private float _percentageOfThermalInFluence; public float PercentageOfThermalInFluence
    { get { return _percentageOfThermalInFluence; } }
    [SerializeField]
    private ParticleSystem _steam;
    public void Evaporation()
    {
        if (_steam != null)
        {
            _steam.transform.SetParent(null);
            _steam.Play();
            Destroy(_steam.gameObject, 1);
        }
        Destroy(gameObject);
    }
}
