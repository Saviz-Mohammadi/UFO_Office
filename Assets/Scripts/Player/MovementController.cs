using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour {

    [Header("Dependencies")]
    [Tooltip("Required for movement to work.")]
    [SerializeField] private CharacterController _characterController = null;


    [Header("Fields (Read-Only)")]
    [Tooltip("Represents the final movement vector calculated for vertical movement. (Targeted by gravity and jumping)")]
    [SerializeField] private Vector3 _verticalVelocity = Vector3.zero;

    [Tooltip("Represents the final movement vector calculated for horizontal movement.")]
    [SerializeField] private Vector3 _horizontalVelocity = Vector3.zero;

    [Space(10)]
    [Tooltip("Represents the grounded state. (Becomes true when player is located on a ground surface.)")]
    [SerializeField] private bool _isGrounded = false;
    
    [Tooltip("Represents the sprinting state.")]
    [SerializeField] private bool _isSprinting = false;
    
    [Tooltip("Represents the jumping state.")]
    [SerializeField] private bool _isJumping = false;
    
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls the normal speed of the player.")]
    [SerializeField] private float _normalSpeed = 2.0f;
    
    [Tooltip("Controls the sprint speed of the player.")]
    [SerializeField] private float _sprintSpeed = 4.0f;
    
    [Space(10)]
    [Tooltip("Controls the jump height of the player.")]
    [SerializeField] private float _jumpHeight = 1.0f;
    
    [Tooltip("Controls the gravity imposed on the player.")]
    [SerializeField] private float _gravityValue = -9.81f;

    

    private void Update() {

        PerformHorizontalMovement();
        PerformVerticalMovement();
    }

    
    
    private void PerformHorizontalMovement() {

        Vector3 moveDirection = (transform.forward * _horizontalVelocity.z) + (transform.right * _horizontalVelocity.x);
        
        if (_isSprinting) {
            
            _characterController.Move(moveDirection * (Time.deltaTime * _sprintSpeed));
        }

        else {

            _characterController.Move(moveDirection * (Time.deltaTime * _normalSpeed));
        }
    }

    private void PerformVerticalMovement() {
        
        if (_isGrounded && _verticalVelocity.y < 0.0f) {
            
            _verticalVelocity.y = -2.0f;
        }
        
        if (_isJumping && _isGrounded) {
            
            _verticalVelocity.y += Mathf.Sqrt(_jumpHeight * -2.0f * _gravityValue);
        }
        
        _verticalVelocity.y += _gravityValue * Time.deltaTime;
        
        _characterController.Move(_verticalVelocity * Time.deltaTime);
        
        // WARNING (SAVIZ): It is always more accurate to check for 'isGrounded' after performing the 'Move()' operation. (This caused me so much pain)
        _isGrounded = _characterController.isGrounded;
    }
    
    // Called on 'Moved' Unity-Event of 'Player Input' component:
    public void OnMoved(InputAction.CallbackContext context) {
        
        Vector2 _inputMovementVector =  context.ReadValue<Vector2>().normalized;

        _horizontalVelocity = new Vector3(_inputMovementVector.x, 0.0f, _inputMovementVector.y);
    }
    
    // Called on 'Sprinted' Unity-Event of 'Player Input' component:
    public void OnSprinted(InputAction.CallbackContext context) {

        if (context.phase != InputActionPhase.Canceled) {
            
            _isSprinting = true;
            return;
        }
        
        _isSprinting = false;
    }
    
    // Called on 'Jumped' Unity-Event of 'Player Input' component:
    public void OnJumped(InputAction.CallbackContext context) {

        if (context.phase != InputActionPhase.Canceled) {
            
            _isJumping = true;
            return;
        }
        
        _isJumping = false;
    }
}
