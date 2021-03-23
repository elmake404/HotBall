using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLife : MonoBehaviour
{
    [SerializeField]
    private Material _coldMaterial;
    private Material _mainMaterial;
    private Color _colorCold, _colorHot, _differenceColor;
    [SerializeField]
    private MeshRenderer _mesh;
    [SerializeField]
    private Light _light;
    [SerializeField]
    private float _timeToCoolDown = 10;
    private float _timerToCoolDown;
    void Start()
    {
        _timerToCoolDown = _timeToCoolDown;
        _mainMaterial = new Material(_mesh.material);

        _mesh.material = _mainMaterial;
        _colorCold = _coldMaterial.color;
        _colorHot = _mainMaterial.color;
        _differenceColor = _colorCold - _colorHot;
    }

    private void FixedUpdate()
    {
        if (CanvasManager.IsGameFlow)
        {
            EffectsControl();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Liquid liquid = collision.collider.GetComponent<Liquid>();

        if (liquid != null)
        {
            TemperatureRegulator(liquid.PercentageOfThermalInFluence);
            liquid.Evaporation();
        }

        if (collision.collider.tag == "Finish")
        {
            CanvasManager.IsGameFlow = false;
            CanvasManager.IsWinGame = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Finish")
        {
            CanvasManager.IsGameFlow = false;
            CanvasManager.IsWinGame = true;
        }
    }
    private void EffectsControl()
    {
        if (_timerToCoolDown > 0)
        {
            _timerToCoolDown -= Time.deltaTime;
        }
        else
        {
            CanvasManager.IsLoseGame = true;
            CanvasManager.IsGameFlow = false;
        }

        _mainMaterial.color = _colorHot + (_differenceColor / 100) * ((_timeToCoolDown - _timerToCoolDown) / _timeToCoolDown * 100);
        _light.range = (_timerToCoolDown / _timeToCoolDown);
    }
    private void TemperatureRegulator(float percentageOfThermalInFluence)
    {
        _timerToCoolDown += _timeToCoolDown / 100f * percentageOfThermalInFluence;

        if (_timerToCoolDown > _timeToCoolDown)
        {
            _timerToCoolDown = _timeToCoolDown;
        }
    }
}
