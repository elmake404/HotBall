using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    [SerializeField]
    private float _speedNormal, _speedMetal, _speedPlastic;
    private float _speed;
    private Vector3 _direction = Vector3.down;
    private Vector3 _startTouchPos, _currentTouchPos;

    void Start()
    {
        _speed = _speedNormal;
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

                _direction.x = (_currentTouchPos - _startTouchPos).x / 40;
                if (Mathf.Abs(_direction.x) > 1)
                {
                    int diferens = _direction.x > 0 ? 1 : -1;
                    _direction.x = 1 * diferens;
                }
            }
        }
        else
        {
            _direction = Vector3.down;
        }

    }
    void FixedUpdate()
    {
        transform.Translate(_direction * _speed);
    }
    public Vector2 GetDirectionMove()
    {
        return _direction;
    }
}
