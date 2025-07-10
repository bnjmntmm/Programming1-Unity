using System.Collections;
using UnityEngine;

public class MovingPlatformAnimation : MonoBehaviour
{
    [SerializeField] private Animation _moveAnimation;
    [SerializeField] [Range(0f, 20f)] private float _animationDelay;

    private void Start()
    {
        if(_animationDelay != 0f) StartCoroutine(StartAnimationRoutine());
        else _moveAnimation.Play();
    }

    private IEnumerator StartAnimationRoutine()
    {
        yield return new WaitForSeconds(_animationDelay);
        _moveAnimation.Play();
    }
}