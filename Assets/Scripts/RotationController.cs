using System;
using UnityEngine;
using UnityEngine.Serialization;

// NOTE (SAVIZ): 'Time.deltaTime' is not used in this module, because mouse rotation is not affected as heavily as movement.

public class RotationController : MonoBehaviour {

    [Header("Dependencies")]
    [Tooltip("Required for obtaining input for mouse movement.")]
    [SerializeField] private InputController _inputController = null;
    
    [Tooltip("Select the 'Transform' of the 'Camera' associated with the 'Player' character.")]
    [SerializeField] private Transform _cameraTransform = null;
    
    [Tooltip("Select the 'Transform' that contains the entire 'Player' skeleton.")]
    [SerializeField] private Transform _playerTransform = null;

    [Header("Rotation Settings")]
    [Tooltip("Controls horizontal rotation speed.")]
    [SerializeField] private float _sensitivityX = 5.0f;
    
    [Tooltip("Controls vertical rotation speed.")]
    [SerializeField] private float _sensitivityY = 5.0f;
    
    [Tooltip("Controls maximum vertical pitch.")]
    [SerializeField] private float _verticalRotationLimit = 85.0f;
    
    [Header("Fields (Read-Only)")]
    [Tooltip("Tracks vertical pitch for 'Camera'.")]
    [SerializeField] private float _verticalRotation = 0.0f;
    
    void Update() {
        
        RotatePlayer();
        RotateCamera();
    }

    private void RotatePlayer() {
        
        _playerTransform.rotation *= Quaternion.Euler(0.0f, _inputController.ObtainNormalizedMouseX() * _sensitivityX, 0.0f);
    }

    private void RotateCamera() {
        
        _verticalRotation -= _inputController.ObtainNormalizedMouseY() * _sensitivityY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalRotationLimit, _verticalRotationLimit);

        // Rotate Camera (Pitch + Yaw)
        _cameraTransform.localRotation = Quaternion.Euler(_verticalRotation, 0.0f, 0.0f);
    }
}
