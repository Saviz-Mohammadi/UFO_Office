using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {
    
    [Header("Dependencies")]
    [Tooltip("Required for obtaining input for movement.")]
    [SerializeField] private PlayerInput _playerInput = null;
    
    [Header("Fields (Read-Only) - Movement")]
    [Tooltip("Shows the movement vector.")]
    [SerializeField] private Vector2 _inputMovementVector = new Vector2(0.0f, 0.0f);

    [Header("Fields (Read-Only) - Rotation")]
    [Tooltip("Shows the Rotation vector.")]
    [SerializeField] private Vector2 _inputRotationVector = new Vector2(0.0f, 0.0f);
    

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
