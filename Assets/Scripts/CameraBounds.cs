using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBounds : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _player;

    [Header("World Bounds (in world units)")]
    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;

    private Camera _camera;
    private float _halfHeight, _halfWidth;

    void Awake()
    {
        _camera = GetComponent<Camera>();

        // for the orthographic camera:
        _halfHeight = _camera.orthographicSize;
        _halfWidth  = _halfHeight * _camera.aspect;
    }

    void LateUpdate()
    {
        // 1. Desired camera pos (match player, keep current z)
        Vector3 desired = new Vector3(_player.position.x,
            _player.position.y,
            transform.position.z);

        // 2. Clamp each axis so the camera frame never shows outside your bounds.
        float clampedX = Mathf.Clamp(
            desired.x,
            _minBounds.x + _halfWidth,
            _maxBounds.x - _halfWidth
        );
        float clampedY = Mathf.Clamp(
            desired.y,
            _minBounds.y + _halfHeight,
            _maxBounds.y - _halfHeight
        );

        // 3. Move the camera
        transform.position = new Vector3(clampedX, clampedY, desired.z);
    }
}