using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    [SerializeField]
    private Animator _animation;

    private void FixedUpdate()
    {
        if (CanvasManager.IsStartGeme && _animation != null && !_animation.enabled)
        {
            StartAnimation();
        }

    }
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(_animation.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Destroy(_animation);
        CanvasManager.IsGameFlow = true;
    }

    private void StartAnimation()
    {
        _animation.enabled = true;
        StartCoroutine(StartGame());
    }

}
