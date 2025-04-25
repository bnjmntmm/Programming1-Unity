using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    
    
    
    
    private InputAction _moveAction;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = _moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(moveValue.x, 0, moveValue.y);

        transform.position += moveDirection * (_speed * Time.deltaTime);

        
        
        
        // if (Input.GetKey(KeyCode.W))
        // {
        //     transform.position += Vector3.forward * speed * Time.deltaTime;
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     transform.position += Vector3.back * speed * Time.d   eltaTime;
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     transform.position += Vector3.left * speed * Time.deltaTime;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     transform.position += Vector3.right * speed * Time.deltaTime;
        // }
        // // translate = dir * speed
    }

}
