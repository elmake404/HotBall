using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Vector3 _direction = Vector3.down;
    private Vector3 _startTouchPos, _currentTouchPos, _directionCurrent = Vector3.down,_startPosBall;
    private List<MaterialCharacteristics> _materialCharacteristics = new List<MaterialCharacteristics>();
    [SerializeField]
    private RuntimeCircleScorcher _runtimeCircle;
    [SerializeField]
    private CircleCollider2D _colliderMain;

    [SerializeField]
    private float _speed, _limmitHorizontal = 6;
    private bool _isHole;

    void Start()
    {
        _startPosBall = transform.position;
    }
    private void Update()
    {
        if (Move()&&_runtimeCircle.ContactDestructibleTerrain())
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
                    if (_startTouchPos == Vector3.zero)
                    {
                        _startTouchPos = touch.position;
                    }

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


    }
    void FixedUpdate()
    {
        if (Move())
        {
            if (_direction != _directionCurrent)
            {
                _direction = Vector3.Slerp(_direction, _directionCurrent, 0.5f);
            }

            transform.Translate(_direction * (_speed - SpeedPenalty()));
            CorrectionPosition();
        }
    }
    private float SpeedPenalty()
    {
        float speedPenaltyProcent = 0;
        if (!_isHole)
        {
            for (int i = 0; i < _materialCharacteristics.Count; i++)
            {
                if (speedPenaltyProcent < _materialCharacteristics[i].SpeedPenalty)
                {
                    speedPenaltyProcent = _materialCharacteristics[i].SpeedPenalty;
                }
            }
        }

        return (_speed / 100) * speedPenaltyProcent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var materialCharacteristics = collision.GetComponent<MaterialCharacteristics>();
        if (materialCharacteristics != null)
        {
            _materialCharacteristics.Add(materialCharacteristics);
        }
        if (collision.tag == "Hole")
        {
            _isHole = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var materialCharacteristics = collision.GetComponent<MaterialCharacteristics>();
        if (materialCharacteristics != null)
        {
            _materialCharacteristics.Remove(materialCharacteristics);
        }
        if (collision.tag == "Hole")
        {
            _isHole = false;
        }

    }
    private void CorrectionPosition()
    {
        Vector3 PosCorrection = transform.position;

        if (PosCorrection.x >_startPosBall.x+_limmitHorizontal)
        {
            PosCorrection.x = _startPosBall.x + _limmitHorizontal;
        }
        else if (PosCorrection.x < _startPosBall.x - _limmitHorizontal)
        {
            PosCorrection.x = _startPosBall.x - _limmitHorizontal;
        }
        transform.position = PosCorrection;
    }

    private bool Move()
    {
        return (CanvasManager.IsGameFlow && CanvasManager.IsStartGeme);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position - new Vector3(_limmitHorizontal, 0, 0), transform.position + new Vector3(_limmitHorizontal, 0, 0));
    }

    public float GetRadius()
    {
        return _colliderMain.radius;
    }
    public Vector2 GetDirectionMove()
    {
        return _direction;
    }

}
