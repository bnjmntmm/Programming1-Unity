using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Camera _camera;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _dashAction;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpVelocity = 5f;
    [SerializeField] private float _gravityMultiplier = 1f;
    [SerializeField] private float _dashVelocity = 5f;
    [SerializeField] private float _dashingTime = 1f;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private Vector3 _frameMovement;

    [Header("Orbit Settings")] [SerializeField]
    private float _distance = 5f;

    private float _dashingTimer = 0f;
    private float _xVelocity = 0f;
    private float _yVelocity = 0f;
    private bool _isJumping = false;
    private bool _isDashing = false;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void OnEnable()
    {
        _jumpAction.action.performed += OnJumpAction;
        _dashAction.action.performed += OnDashAction;
    }

    private void OnDashAction(InputAction.CallbackContext context)
    {
        if (!context.performed || _isDashing) return;
        Debug.Log("Dash");
        _isDashing = true;
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        var startTime = Time.time;
        var direction = _controller.velocity.x > 0 ? Vector3.right : Vector3.left;
        while (startTime + _dashingTime > Time.time)
        {
            _controller.Move(direction * (_dashVelocity * Time.deltaTime));
            yield return null;
        }

        _isDashing = false;
    }

    private void OnJumpAction(InputAction.CallbackContext context)
    {
        if (!context.performed || _isJumping) return;
        Debug.Log("Jump");
        _yVelocity += _jumpVelocity;
        _isJumping = true;
    }

    private void Update()
    {
        var moveValue = _moveAction.action.ReadValue<Vector2>();
        var frameMovement = Vector3.zero;

        if (_controller.isGrounded == false)
        {
            _yVelocity += Physics.gravity.y * _gravityMultiplier * Time.deltaTime;
            _isGrounded = false;
        }
        else if (_isJumping == false)
        {
            _yVelocity = -0.01f;
        }

        if (moveValue != Vector2.zero && !_isDashing)
        {
            var worldMoveDir = transform.right * moveValue.x;
            // transform.position += worldMoveDir * (_speed * Time.deltaTime);
            frameMovement = worldMoveDir * (_speed * Time.deltaTime);
        }

        // if (_isDashing)
        // {
        //     if (frameMovement.x > 0)
        //     {
        //     }
        //
        //     _xVelocity += _dashVelocity;
        //     _dashingTimer += Time.deltaTime;
        //     if (_dashingTimer >= _dashingTime)
        //     {
        //         _isDashing = false;
        //     }
        // }

        frameMovement.y += _yVelocity * Time.deltaTime;
        _controller.Move(frameMovement);
        _frameMovement = frameMovement;

        _camera.orthographicSize = _distance;

        if (!_controller.isGrounded) return;
        _isGrounded = true;
        _isJumping = false;
    }
}