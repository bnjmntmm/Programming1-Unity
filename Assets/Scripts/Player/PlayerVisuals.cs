using System;
using System.Collections;
using Player;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    
    [SerializeField]
    private Animator _playerAnimator;
    
    [SerializeField]
    private PlayerController _playerController;

    private PlayerAnimationStates.States _currentAnimationState = PlayerAnimationStates.States.Start;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeAnimation(PlayerAnimationStates.States.Idle);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckAnimation()
    {
        if (_currentAnimationState == PlayerAnimationStates.States.Land || _currentAnimationState == PlayerAnimationStates.States.Jump)
        {
            return;
        }

        if (_currentAnimationState == PlayerAnimationStates.States.Fall)
        {
            if (_playerController.IsGrounded)
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
                
                
                Debug.Log(_currentAnimationState);
                _playerAnimator.CrossFade(PlayerAnimationStates.ConvertToString(_currentAnimationState), crossfade);
            }
        }
        
    }
    
}
