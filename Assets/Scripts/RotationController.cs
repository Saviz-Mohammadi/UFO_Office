using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// NOTE (SAVIZ): 'Time.deltaTime' is not used in this module, because mouse rotation is not affected as heavily as movement.

public class RotationController : MonoBehaviour {
    
    #region Fields
    [Header("Dependencies")]
    [Tooltip("Required for obtaining input.")]
    [SerializeField] private InputController _inputController = null;
    
    [Tooltip("The selected 'Transform' placed in this field will be the targeted as 'Camera' for vertical rotation.")]
    [SerializeField] private Transform _cameraTransform = null;
    
    [Tooltip("The selected 'Transform' placed in this field will be the targeted as 'Visual' for horizontal rotation.")]
    [SerializeField] private Transform _playerTransform = null;

    
    
    [Header("Fields (Read-Only)")]
    [Tooltip("Tracks vertical pitch for 'Camera'.")]
    [SerializeField] private float _verticalRotation = 0.0f;
    
    
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls the horizontal rotation speed.")]
    [SerializeField] private float _sensitivityX = 2.0f;
    
    [Tooltip("Controls vertical rotation speed.")]
    [SerializeField] private float _sensitivityY = 2.0f;
    
    [Tooltip("Controls maximum vertical pitch.")]
    [SerializeField] private float _verticalRotationLimit = 85.0f;
    
    [SerializeField] private float _horizontalRotation = 0.0f;
    #endregion

    
    #region Unity Events

    private void Awake() {
        
        Cursor.lockState = CursorLockMode.Locked;
        // Make the cursor invisible
        Cursor.visible = false;
    }

    void Update() {
        
        RotatePlayer();
    }

    // Updating the camera rotation in LateUpdate is way better to stop the jitter. Because the camera has aready doen all of its movement in the Update call in movenment script.
    void LateUpdate() {
        
        RotateCamera();
    }
    #endregion

    
    #region RotationLogic
    private void RotatePlayer() {
        
        _playerTransform.rotation *= Quaternion.Euler(0.0f, _inputController.ObtainNormalizedMouseX() * Time.deltaTime * _sensitivityX, 0.0f);
    }

    private void RotateCamera() {
        
        _verticalRotation -= _inputController.ObtainNormalizedMouseY() * Time.deltaTime * _sensitivityY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalRotationLimit, _verticalRotationLimit);

        _horizontalRotation += _inputController.ObtainNormalizedMouseX() * Time.deltaTime * _sensitivityX;
        
        // Rotate Camera
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0.0f);
    }
    #endregion
}
