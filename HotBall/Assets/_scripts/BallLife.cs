using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLife : MonoBehaviour
{
    [SerializeField]
    private float _timeToCoolDown = 10;
    private float _timerToCoolDown;
    void Start()
    {
        _timerToCoolDown = _timeToCoolDown;
    }

    private void FixedUpdate()
    {
        if (_timerToCoolDown > 0)
        {
            _timerToCoolDown -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Lose " + _timerToCoolDown);
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
