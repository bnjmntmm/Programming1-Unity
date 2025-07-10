using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    [SerializeField] private ParticleSystem _dashParticles;

    [Header("Orbit Settings")] [SerializeField]
    private float _distance = 5f;

    private bool _isDashing;
    private bool _isJumping;
    private bool _isMovingRight;

    private float _yVelocity;
    public static PlayerController Instance { get; private set; }

    public CharacterController Controller => _controller;
    public bool IsGrounded => _isGrounded;
    public Vector3 FrameMovement => _frameMovement;

    private void Awake()
    {
        if(Instance && Instance != this) Destroy(gameObject);

        Instance = this;
    }

    private void Update()
    {
        var moveValue = _moveAction.action.ReadValue<Vector2>();
        var frameMovement = Vector3.zero;

        if(_controller.isGrounded == false)
        {
            _yVelocity += Physics.gravity.y * _gravityMultiplier * Time.deltaTime;
            _isGrounded = false;
        }
        else if(_isJumping == false)
        {
            _yVelocity = -0.01f;
        }

        if(moveValue != Vector2.zero && !_isDashing)
        {
            var worldMoveDir = transform.right * moveValue.x;
            _isMovingRight = moveValue.x > 0;
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

        if(transform.position.y < -20f)
        {
            _yVelocity = 0f;
            transform.position = new Vector3(0f, 2f, 0f);
        }

        if(!_controller.isGrounded) return;
        _isGrounded = true;
        _isJumping = false;
    }

    private void OnEnable()
    {
        _jumpAction.action.performed += OnJumpAction;
        _dashAction.action.performed += OnDashAction;
    }

    private void OnDashAction(InputAction.CallbackContext context)
    {
        if(!context.performed || _isDashing) return;
        // Debug.Log("Dash");
        _isDashing = true;
        _dashParticles.transform.rotation = _isMovingRight
            ? Quaternion.Euler(0f, -90f, 0f)
            : _dashParticles.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        _dashParticles.Play();
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        var startTime = Time.time;
        var direction = _isMovingRight ? Vector3.right : Vector3.left;
        while(startTime + _dashingTime > Time.time)
        {
            _controller.Move(direction * (_dashVelocity * Time.deltaTime));
            yield return null;
        }

        _isDashing = false;
    }

    private void OnJumpAction(InputAction.CallbackContext context)
    {
        if(!context.performed || _isJumping) return;
        // Debug.Log("Jump");
        _yVelocity = _jumpVelocity;
        _isJumping = true;
    }
}