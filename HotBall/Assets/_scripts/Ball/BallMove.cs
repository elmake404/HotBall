using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour
{
    private Vector3 _direction = Vector3.down;
    private Vector3 _startTouchPos, _startPosBall;
    private Vector3 _currentPosBall, _targetPosBall;
    private List<MaterialCharacteristics> _materialCharacteristics = new List<MaterialCharacteristics>();
    [SerializeField]
    private CircleCollider2D _colliderMain;
    private Camera _cam;

    [SerializeField]
    private float _speed, _limmitHorizontal = 6;
    private bool _isHole;

    void Start()
    {
        _targetPosBall = transform.position;
        _cam = Camera.main;
        _startPosBall = transform.position;
    }
    private void Update()
    {
        if (Move())
        {
            if (TouchUtility.TouchCount > 0 && ControlBall())
            {
                Touch touch = TouchUtility.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                    _currentPosBall = transform.position;
                    _startTouchPos = (_cam.transform.position - ((ray.direction) *
                            ((_cam.transform.position - transform.position).z / ray.direction.z)));
                }
                else if (touch.phase == TouchPhase.Moved)
                { 
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                    if (_startTouchPos == Vector3.zero)
                    {
                        _startTouchPos = (_cam.transform.position - ((ray.direction) *
                                ((_cam.transform.position - transform.position).z / ray.direction.z)));
                    }

                    _targetPosBall = _currentPosBall + ((_cam.transform.position - ((ray.direction) *
                        ((_cam.transform.position - transform.position).z / ray.direction.z))) - _startTouchPos);
                }
            }
            else
            {
                _targetPosBall = transform.position;
            }
        }


    }
    void FixedUpdate()
    {
        if (Move())
        {
            transform.Translate(_direction * (_speed - SpeedPenalty()));

            Vector3 PosX = transform.position;
            PosX.x = _targetPosBall.x;
            transform.position = Vector3.MoveTowards(transform.position, PosX, (_speed - SpeedPenalty()));

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
            if (materialCharacteristics.IsDenseMetal())
                materialCharacteristics.HoleCreation(transform.position);
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

        if (PosCorrection.x > _startPosBall.x + _limmitHorizontal)
        {
            PosCorrection.x = _startPosBall.x + _limmitHorizontal;
        }
        else if (PosCorrection.x < _startPosBall.x - _limmitHorizontal)
        {
            PosCorrection.x = _startPosBall.x - _limmitHorizontal;
        }
        transform.position = PosCorrection;
    }
    private bool ControlBall()
    {
        for (int i = 0; i < _materialCharacteristics.Count; i++)
        {
            if (_materialCharacteristics[i].IsDenseMetal())
                return false;
        }
        return true;
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
    public float GetRadius() => _colliderMain.radius;
    public Vector2 GetDirectionMove()
    {
        Vector3 direction = (_targetPosBall - transform.position).normalized; 
        return new Vector3(direction.x,_direction.y,0).normalized;
    }

}
