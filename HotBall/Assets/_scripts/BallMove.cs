﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Vector3 _direction = Vector3.down;
    private Vector3 _startTouchPos, _currentTouchPos, _directionCurrent;
    private List<MaterialCharacteristics> _materialCharacteristics = new List<MaterialCharacteristics>();

    [SerializeField]
    private float _speed;


    void Start()
    {

    }
    private void Update()
    {
        if (TouchUtility.TouchCount > 0)
        {
            Touch touch = TouchUtility.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                _currentTouchPos = touch.position;

                _directionCurrent.x = (_currentTouchPos - _startTouchPos).x / 40;
                if (Mathf.Abs(_directionCurrent.x) > 1)
                {
                    int diferens = _directionCurrent.x > 0 ? 1 : -1;
                    _directionCurrent.x = 1 * diferens;
                }
            }
        }
        else
        {
            _directionCurrent = Vector3.down;
        }

    }
    void FixedUpdate()
    {
        if (_direction != _directionCurrent)
        {
            _direction = Vector3.Slerp(_direction, _directionCurrent, 0.5f);
        }
        transform.Translate(_direction * (_speed - SpeedPenalty()));
    }
    private float SpeedPenalty()
    {
        float speedPenaltyProcent = 0;
        for (int i = 0; i < _materialCharacteristics.Count; i++)
        {
            if (speedPenaltyProcent< _materialCharacteristics[i].SpeedPenalty)
            {
                speedPenaltyProcent = _materialCharacteristics[i].SpeedPenalty;
            }
        }
        return (_speed / 100) * speedPenaltyProcent;
    }
    public Vector2 GetDirectionMove()
    {
        return _direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       var materialCharacteristics = collision.GetComponent<MaterialCharacteristics>();
        if (materialCharacteristics!=null)
        {
            _materialCharacteristics.Add(materialCharacteristics);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       var materialCharacteristics = collision.GetComponent<MaterialCharacteristics>();
        if (materialCharacteristics!=null)
        {
            _materialCharacteristics.Remove(materialCharacteristics);
        }
    }
}
