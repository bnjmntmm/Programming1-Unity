using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    
    [SerializeField] private float _speed = 5f;

    [Header("Orbit Settings")] [SerializeField]
    private float _distance = 5f;

    public bool IsGrounded;

    private void OnEnable()
    {
        _jumpAction.action.performed += OnJumped;
    }

    private void OnJumped(InputAction.CallbackContext context)
    {
        if(context.performed) Debug.Log("Jump");
    }

    private void Update()
    {
        var moveValue = _moveAction.action.ReadValue<Vector2>();

        if (moveValue != Vector2.zero)
        {
            var worldMoveDir = transform.right * moveValue.x;
            transform.position += worldMoveDir * (_speed * Time.deltaTime);
        }

        _camera.orthographicSize = _distance;
    }
}