using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveControl : MonoBehaviour
{
    [SerializeField]
    private Transform _target, _startPosCamera;
    [SerializeField]
    private Vector3 _ofset, _velocity, _gamePosCamera;

    [SerializeField]
    private float _speedMove;

    private void Awake()
    {
        _ofset = transform.position - _target.position;
        _gamePosCamera = transform.position;
        transform.position = _startPosCamera.position;
    }


    private void FixedUpdate()
    {
        //if (CanvasManager.IsStartGeme && !CanvasManager.IsGameFlow)
        //{
        //    StartAnimation();
        //}

        if (CanvasManager.IsGameFlow && CanvasManager.IsStartGeme)
        {
            transform.position = MoveCamera();
        }
    }
    private Vector3 MoveCamera()
    {
        Vector3 currentPosCamera = transform.position;
        currentPosCamera.y = (_target.position + _ofset).y;
        return Vector3.SmoothDamp(transform.position, currentPosCamera, ref _velocity, _speedMove);
    }
    private IEnumerator GoToGamePosCamera(float travelTime)
    {
        float _speed = (transform.position - _gamePosCamera).magnitude / (travelTime * 60);
        Debug.Log((transform.position - _gamePosCamera).magnitude);
        Debug.Log(travelTime);
        Debug.Log(_speed);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _gamePosCamera, _speed);
            if ((transform.position - _gamePosCamera).magnitude <= 0.05f || CanvasManager.IsGameFlow)
            {
                break;
            }
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
    public void StartMoveCamera(float travelTime)
    {
        StartCoroutine(GoToGamePosCamera(travelTime));
    }
}
