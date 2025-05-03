using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    [SerializeField] private float _speed = 5f;
    
    [Header("Orbit Settings")]
    [SerializeField] private float _distance     = 5f;
    [SerializeField] private float _rotationSpeed = 180f;      // degrees per second
    [SerializeField] private float _minPitch      = -30f;
    [SerializeField] private float _maxPitch      =  60f;
    
    private float _pitch;
    private float _yaw;
    
    
    private InputAction _moveAction;

    private InputAction _lookAction;
    private Vector2 _lookValue;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _yaw = transform.rotation.eulerAngles.y;
        _pitch = transform.rotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        // 1) Read input
        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        Vector2 lookValue = _lookAction.ReadValue<Vector2>();

        // 2) Update your orbit angles
        _yaw   += lookValue.x * _rotationSpeed * Time.deltaTime;
        _pitch -= lookValue.y * _rotationSpeed * Time.deltaTime;
        _pitch  = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        // 3) Rotate the player’s body on Y only
        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);

        // 4) Move in the body’s local forward/right
        //    so “W” (moveValue.y > 0) is always in the look‐direction
        Vector3 forward = transform.forward;
        Vector3 right   = transform.right;
        Vector3 worldMoveDir = forward * moveValue.y + right * moveValue.x;
        transform.position += worldMoveDir * (_speed * Time.deltaTime);

        // 5) Orbit the camera around the player
        Quaternion camRot    = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3  camOffset  = camRot * Vector3.back * _distance;
        _camera.transform.position = transform.position + camOffset;
        _camera.transform.rotation = camRot;
    }


}
