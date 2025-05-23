using System;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxEffect : MonoBehaviour
{
    private float _length;
    private float _startPos;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _parallaxEffect;


    void Start()
    {
     _startPos = transform.position.x;
     _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (_camera.transform.position.x * (1 - _parallaxEffect));
        float distance = (_camera.transform.position.x * _parallaxEffect);
        transform.position = new Vector3(_startPos + distance, transform.position.y, transform.position.z);

        if (temp > _startPos * _length)
        {
            _startPos += _length;
        } else if (temp < _startPos + _length)
        {
            _startPos -= _length;
        }
    }
}

