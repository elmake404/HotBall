using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveControl : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    private Vector3 _ofset, _velocity;

    [SerializeField]
    private float _speedMove;
    private float _timeAnimation;

    private void Start()
    {
        _timeAnimation = 1.3f;
        _ofset = transform.position - _target.position;
    }


    private void FixedUpdate()
    {
        if (CanvasManager.IsStartGeme && !CanvasManager.IsGameFlow)
        {
            StartAnimation();
        }

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
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(_timeAnimation);
        CanvasManager.IsGameFlow = true;
    }
    private void StartAnimation()
    {
        StartCoroutine(StartGame());
    }
}
