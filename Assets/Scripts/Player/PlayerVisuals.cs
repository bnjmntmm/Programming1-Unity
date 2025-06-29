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
    [SerializeField] private InputActionReference _moveAction;
    
    private SpriteRenderer _spriteRenderer;
    private PlayerAnimationStates.States _currentAnimationState = PlayerAnimationStates.States.Start;
    private bool _wasGroundedLastFrame = true;
    private Vector2 _moveValue;

    private void OnEnable()
    {
        _jumpAction.action.performed += OnJumpAction;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeAnimation(PlayerAnimationStates.States.Idle);
        
    }

    void Update()
    {
        
        _moveValue= _moveAction.action.ReadValue<Vector2>();
        // flip sprite based on movement direction
        if (_playerController.FrameMovement.x > 0)
        {
            //right movement
            _spriteRenderer.flipX = false;
        }
        else if (_playerController.FrameMovement.x < 0)
        {
            //left movement
            _spriteRenderer.flipX = true;
        }
        
        CheckAnimation();
        _wasGroundedLastFrame = _playerController.Controller.isGrounded;
    }
    

    private void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.performed && _currentAnimationState != PlayerAnimationStates.States.Jump)
        {
           
            ChangeAnimation(PlayerAnimationStates.States.Jump);
        }
    }
    

    private void CheckAnimation()
    {
        if (_currentAnimationState == PlayerAnimationStates.States.Land || _currentAnimationState == PlayerAnimationStates.States.Jump)
            return;

        // Walked off edge: last frame grounded, now not, and NOT jumping
        if (_wasGroundedLastFrame && !_playerController.Controller.isGrounded && _currentAnimationState != PlayerAnimationStates.States.Fall)
        {
            // Optional: also check not jumping, if you track that in PlayerController
            if (!_playerController.IsGrounded) // Or your equivalent
            {
                ChangeAnimation(PlayerAnimationStates.States.Fall);
                return;
            }
        }

        // If currently falling, but landed
        if (_currentAnimationState == PlayerAnimationStates.States.Fall)
        {
            if (_playerController.Controller.isGrounded)
            {
                ChangeAnimation(PlayerAnimationStates.States.Land);
            }
            return;
        }
        
        // check walk / idle if grounded and not falling / jumping
        if (_playerController.Controller.isGrounded)
        {
            if (_moveValue.x != 0 && _currentAnimationState != PlayerAnimationStates.States.Walk)
            {
                ChangeAnimation(PlayerAnimationStates.States.Walk);
            }
            else if (_moveValue.x == 0 && _currentAnimationState != PlayerAnimationStates.States.Idle)
            {
                ChangeAnimation(PlayerAnimationStates.States.Idle);
            }
        }
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
