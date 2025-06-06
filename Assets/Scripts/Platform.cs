using UnityEngine;

public class Platform : MonoBehaviour
{
    
    [SerializeField] private BoxCollider _collider;
    private void Update()
    {
        _collider.isTrigger = PlayerController.Instance.transform.position.y < transform.position.y +1;
    }
}
