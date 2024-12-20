using System;
using UnityEngine;
using UnityEngine.InputSystem;

// NOTE (SAVIZ): We should never multiply mouse input by 'Time.deltaTime' because mouse input is already a position delta.

public class RotationController : MonoBehaviour {
    
    [Header("Dependencies")]
    [Tooltip("The 'Transform' placed in this field will be the treated as the visual head model for vertical rotation.")]
    [SerializeField] private Transform _headTransform = null;

    
    [Header("Fields (Read-Only)")]
    [Tooltip("Represents the rotation vector obtained based on mouse input received.")]
    [SerializeField] private Vector2 _inputRotationVector = Vector2.zero;
    
    [Tooltip("Tracks vertical pitch for 'Camera'.")]
    [SerializeField] private float _verticalRotation = 0.0f;
    
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls the vertical rotation speed.")]
    [SerializeField] private float _sensitivityX = 1.5f;
    
    [Tooltip("Controls horizontal rotation speed.")]
    [SerializeField] private float _sensitivityY = 1.5f;
    
    [Tooltip("Controls maximum vertical pitch.")]
    [SerializeField] private float _verticalRotationLimit = 85.0f;
    

    
    private void Awake() {
        
        // TODO (SAVIZ): Probably better to place this thing in GameManager and ask it to do it for us.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        
        RotatePlayer();
        RotateHead();
    }

    
    
    private void RotatePlayer() {
        
        transform.rotation *= Quaternion.Euler(0.0f, _inputRotationVector.x * (_sensitivityX), 0.0f);
    }

    private void RotateHead() {
        
        _verticalRotation -= _inputRotationVector.y * (_sensitivityY);
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalRotationLimit, _verticalRotationLimit);
        
        _headTransform.localRotation = Quaternion.Euler(_verticalRotation, 0.0f, 0.0f);
    }
    
    // Called on 'Looked' Unity-Event of 'Player Input' component:
    public void OnLooked(InputAction.CallbackContext context) {

        _inputRotationVector = context.ReadValue<Vector2>().normalized;
    }
}
