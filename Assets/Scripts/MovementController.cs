using UnityEngine;

public class MovementController : MonoBehaviour {
    
    [Header("Dependencies")]
    [Tooltip("Required for obtaining input for movement.")]
    [SerializeField] private InputController _inputController = null;
    
    [Tooltip("The 'Transform' of the 'Player'for movement.")]
    [SerializeField] private Transform _transform = null;

    [Header("Movement Settings")]
    [Tooltip("Speed multiplier.")]
    [SerializeField] private float _speed = 5.0f;
    
    [Tooltip("Collision detection.")]
    [SerializeField] private bool _collisionIsEnabled = true;
    
    [Tooltip("Collision height. (Should be close to the actual player height)")]
    [SerializeField] private float _collisionHeight = 2.0f;
    
    [Tooltip("Collision radius. (Should be close to the actual player radius)")]
    [SerializeField] private float _collisionRadius = 1.0f;
    
    private void Update() {
        
        Vector3 targetDirection = (_transform.forward * _inputController.ObtainNormalizedMovementY()) + (_transform.right * _inputController.ObtainNormalizedMovementX());
        
        if (_collisionIsEnabled) {
            
            if (willCollide(targetDirection)) {
                
                return;
            }

            MovePlayer(targetDirection);
            
            return;
        }
        
        // If collision is not enabled, then just move freely.
        MovePlayer(targetDirection);
    }

    private void MovePlayer(Vector3 moveDirection) {
        
        _transform.position += moveDirection * Time.deltaTime * _speed;
    }

    // NOTE (SAVIZ): Due to the way this logic works, it is best to not have a collider for the floor, since there is usually the tendency for the 'Player' to be on y=0.0f, which means this will always collide.
    // NOTE (SAVIZ): I am aware that we are using a 'Cube' as the player model. However, usually it is best to use 'CapsuleCast' for checking collision.
    private bool willCollide(Vector3 checkDirection) {
        
        // TODO (SAVIZ): Make a custom field for controlling the maxDistance.
        return (Physics.CapsuleCast(_transform.position, _transform.position + (Vector3.up * _collisionHeight), _collisionRadius, checkDirection, 0.25f));
    }
}
