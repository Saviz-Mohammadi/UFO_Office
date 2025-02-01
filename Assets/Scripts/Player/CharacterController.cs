using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for movement to work.")]
    [SerializeField] private UnityEngine.CharacterController _characterController = null;
    
    [Tooltip("The 'Transform' placed in this field will be the treated as the 'Camera' object transform.")]
    [SerializeField] private Transform _cameraTransform = null;
    
    [Tooltip("The 'Transform' placed in this field will be the treated as the visual head model for vertical rotation.")]
    [SerializeField] private Transform _headTransform = null;
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls the normal speed of the player.")]
    [SerializeField] private float _normalSpeed = 2.0f;
    
    [Tooltip("Controls the sprint speed of the player.")]
    [SerializeField] private float _sprintSpeed = 4.0f;
    
    [Tooltip("Controls the jump height of the player.")]
    [SerializeField] private float _jumpHeight = 1.0f;
    
    [Tooltip("Controls the gravity imposed on the player.")]
    [SerializeField] private float _gravityValue = -9.81f;
    
    [Tooltip("Controls the vertical rotation speed.")]
    [SerializeField] private float _sensitivityX = 1.5f;
    
    [Tooltip("Controls horizontal rotation speed.")]
    [SerializeField] private float _sensitivityY = 1.5f;
    
    [Tooltip("Controls maximum vertical pitch.")]
    [SerializeField] private float _verticalRotationLimit = 85.0f;
    
    [Tooltip("Controls how smoothly the 'Camera' repositioning movement is.")]
    [SerializeField] private float _positionSmoothTime = 0.125f;
    
    [Tooltip("Controls how smoothly the 'Camera' realigning rotation is")]
    [SerializeField] private float _rotationSmoothSpeed = 10.0f;
    
    private Vector2 _inputRotationVector = Vector2.zero;
    private float _verticalRotation = 0.0f;
    
    private Vector3 _verticalVelocity = Vector3.zero;
    private Vector3 _horizontalVelocity = Vector3.zero;
    
    private Vector3 velocity = Vector3.zero;

    private bool _isGrounded = false;
    private bool _isSprinting = false;
    private bool _isJumping = false;
    
    private void Update() {
        RotatePlayer();
        RotateHead();
        PerformHorizontalMovement();
        PerformVerticalMovement();
    }

    private void LateUpdate() {
        InterpolateRotation();
        InterpolatePosition();
    }

    // Invoked by 'Player Input' component:
    public void OnLooked(InputAction.CallbackContext context) {
        _inputRotationVector = context.ReadValue<Vector2>().normalized;
    }
    
    // Invoked by 'Player Input' component:
    public void OnMoved(InputAction.CallbackContext context) {
        Vector2 _inputMovementVector =  context.ReadValue<Vector2>().normalized;

        _horizontalVelocity = new Vector3(_inputMovementVector.x, 0.0f, _inputMovementVector.y);
    }
    
    // Invoked by 'Player Input' component:
    public void OnSprinted(InputAction.CallbackContext context) {
        _isSprinting = context.ReadValueAsButton();;
    }
    
    // Invoked by 'Player Input' component:
    public void OnJumped(InputAction.CallbackContext context) {
        _isJumping = context.ReadValueAsButton();
    }
    
    private void RotatePlayer() {
        transform.rotation *= Quaternion.Euler(0.0f, _inputRotationVector.x * (_sensitivityX), 0.0f);
    }

    private void RotateHead() {
        _verticalRotation -= _inputRotationVector.y * (_sensitivityY);
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalRotationLimit, _verticalRotationLimit);
        
        _headTransform.localRotation = Quaternion.Euler(_verticalRotation, 0.0f, 0.0f);
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
        
        // WARNING (SAVIZ): It is more accurate to check for 'isGrounded' state after performing the 'Move()' operation. (This caused me so much pain)
        _isGrounded = _characterController.isGrounded;
    }
    
    private void InterpolatePosition() {
        _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, _headTransform.position, ref velocity, _positionSmoothTime);
    }

    private void InterpolateRotation() {
        _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _headTransform.rotation, Time.deltaTime * _rotationSmoothSpeed);
    }
}
