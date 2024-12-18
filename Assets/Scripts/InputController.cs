using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {
    
    #region Fields
    [Header("Dependencies")]
    [Tooltip("Required for obtaining input.")]
    [SerializeField] private PlayerInput _playerInput = null;
    
    
    
    [Header("Fields (Read-Only)")]
    [Tooltip("Represents the movement vector obtained based on input received.")]
    [SerializeField] private Vector2 _inputMovementVector = Vector2.zero;
    
    [Tooltip("Represents the rotation vector obtained based on input received.")]
    [SerializeField] private Vector2 _inputRotationVector = Vector2.zero;
    #endregion

    
    #region MovementInput
    public void OnMoved(InputAction.CallbackContext context) {
        
        _inputMovementVector = context.ReadValue<Vector2>().normalized;
    }

    public Vector3 ObtainNormalizedInputMovementVector() {

        return (new Vector3(_inputMovementVector.x, 0.0f, _inputMovementVector.y));
    }

    public float ObtainNormalizedMovementX() {
        
        return (_inputMovementVector.x);
    }
    
    public float ObtainNormalizedMovementY() {
        
        return (_inputMovementVector.y);
    }
    #endregion

    
    #region RotationInput
    public void OnRotated(InputAction.CallbackContext context) {
        
        _inputRotationVector = context.ReadValue<Vector2>().normalized;
    }
    
    public Vector2 ObtainNormalizedInputRotationVector() {

        return (_inputRotationVector);
    }

    public float ObtainNormalizedMouseX() {
        
        return (_inputRotationVector.x);
    }
    
    public float ObtainNormalizedMouseY() {
        
        return (_inputRotationVector.y);
    }
    #endregion
}
