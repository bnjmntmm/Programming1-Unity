using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisuals : MonoBehaviour
{
    
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private InputActionReference _jumpAction;
    private PlayerAnimationStates.States _currentAnimationState = PlayerAnimationStates.States.Start;


    private void OnEnable()
    {
        _jumpAction.action.performed += OnJumpAction;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeAnimation(PlayerAnimationStates.States.Idle);
        
    }

    void Update()
    {
        if(!_playerController.Controller.isGrounded && _currentAnimationState != PlayerAnimationStates.States.Fall)
        {
            ChangeAnimation(PlayerAnimationStates.States.Fall);
        } else if(_playerController.Controller.isGrounded && _currentAnimationState == PlayerAnimationStates.States.Fall)
        {
            ChangeAnimation(PlayerAnimationStates.States.Land);
        } 
    }
    

    private void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.performed && _currentAnimationState != PlayerAnimationStates.States.Jump)
        {
            Debug.Log("Jump Anim");
            ChangeAnimation(PlayerAnimationStates.States.Jump);
        }
    }
    
    

    private void CheckAnimation()
    {
        if (_currentAnimationState == PlayerAnimationStates.States.Land || _currentAnimationState == PlayerAnimationStates.States.Jump)
        {
            return;
        }

        if (_currentAnimationState == PlayerAnimationStates.States.Fall)
        {
            if (_playerController.Controller.isGrounded)
            {
                ChangeAnimation(PlayerAnimationStates.States.Land);
            }
        }


        // if (_playerController.moving)
        // {
        //     ChangeAnimation(WALK);
        // }
        // else
        // {
        //     ChangeAnimation(IDLE);
        // }
    }
    
    public void ChangeAnimation(PlayerAnimationStates.States state, float crossfade = 0.2f, float time = 0)
    {
        if (time > 0) StartCoroutine(Wait());
        else Validate();

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(time-crossfade);
            Validate();
        }

        void Validate()
        {
            if (_currentAnimationState != state)
            {
                _currentAnimationState = state;
                if (_currentAnimationState == 0)
                {
                    CheckAnimation();
                }
                
                _playerAnimator.CrossFade(PlayerAnimationStates.ConvertToString(_currentAnimationState), crossfade);
            }
        }
        
    }
    
}
