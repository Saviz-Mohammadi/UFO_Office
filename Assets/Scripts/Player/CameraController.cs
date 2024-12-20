using System;
using UnityEngine;

// NOTE (SAVIZ): It is recommended to keep the Camera object separate and not as a child of the 'Player Container' for the best result. (I have experienced a lot of jitter otherwise)

public class CameraController : MonoBehaviour
{
    [Header("Dependencies")]
    [Tooltip("The 'Transform' placed in this field will be the treated as the 'Camera' object transform.")]
    [SerializeField] private Transform _cameraTransform = null;
    
    [Tooltip("The 'Transform' placed in this field will be the treated as the follow and rotation target.")]
    [SerializeField] private Transform _targetTransform = null;
    
    [Tooltip("The 'GameObject' placed in this field will be deactivated to prevent collision with 'Camera'.")]
    [SerializeField] private GameObject _visualModel = null;
    
    
    [Header("Fields (Customizable)")]
    [Tooltip("Represents the velocity calculation for positioning.")]
    [SerializeField] private Vector3 velocity = Vector3.zero;
    
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls how smoothly the 'Camera' repositioning movement is.")]
    [SerializeField] private float _positionSmoothTime = 0.125f;
    
    [Tooltip("Controls how smoothly the 'Camera' realigning rotation is")]
    [SerializeField] private float _rotationSmoothSpeed = 10.0f;


    
    private void Awake() {
        
        // TODO (SAVIZ): Here you can perform a check and see if 'IsOwner()' is true or not and only deactivate the current visual model.
        _visualModel.SetActive(false);
    }

    private void LateUpdate() {
        
        InterpolatePosition();
        InterpolateRotation();
    }

    private void InterpolatePosition() {
        
        _cameraTransform.position = Vector3.SmoothDamp(_cameraTransform.position, _targetTransform.position, ref velocity, _positionSmoothTime);
    }

    private void InterpolateRotation() {
        
        _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _targetTransform.rotation, Time.deltaTime * _rotationSmoothSpeed);
    }
}
