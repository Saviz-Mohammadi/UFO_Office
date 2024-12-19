using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementController : MonoBehaviour {

    #region Fields
    [Header("Dependencies")]
    [Tooltip("Required for obtaining input.")]
    [SerializeField] private InputController _inputController = null;
    
    [Tooltip("The selected 'Transform' placed in this field will be the targeted for movement.")]
    [SerializeField] private Transform _transform = null;

    [SerializeField] private Transform _transformCamera = null;
    
    
    [Header("Fields (Read-Only)")]
    [Tooltip("Represents the movement vector obtained based on input received.")]
    [SerializeField] private Vector3 _moveDirection = Vector3.zero;
    
    
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls the speed of the movement.")]
    [SerializeField] private float _speed = 5.0f;
    
    [Space(15)]
    [Tooltip("Use this field to enable or disable the collision detection.")]
    [SerializeField] private bool _collisionIsEnabled = true;
    
    [Tooltip("Controls the height associated with the collision detection. (Should ideally be the same as object actual height)")]
    [SerializeField] private float _collisionHeight = 2.0f;
    
    [Tooltip("Controls the radius associated with the collision detection. (Should ideally be the same as object actual radius)")]
    [SerializeField] private float _collisionRadius = 1.0f;

    [Space(15)]
    [Tooltip("Use this field to enable or disable drawing Gizmos.")]
    [SerializeField] private bool _gizmosIsEnabled = true;
    
    [Tooltip("Controls the color associated with the actual location of the collision detection Gizmos.")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _gizmosColorActual = Color.green;
    
    [Tooltip("Controls the color associated with the predicted location of the collision detection Gizmos.")]
    [ColorUsage(true, true)]
    [SerializeField] private Color _gizmosColorPredicted = Color.red;
    #endregion

    
    #region Unity Events
    private void Update() {
        
        _moveDirection = (_transform.forward * _inputController.ObtainNormalizedMovementY()) + (_transform.right * _inputController.ObtainNormalizedMovementX());
        
        if (_collisionIsEnabled) {
            
            if (willCollide()) {
                
                return;
            }

            MovePlayer();
            
            return;
        }
        
        // If collision is not enabled, then just move freely.
        MovePlayer();
    }
    
    private void OnDrawGizmos() {
        
        if (!_gizmosIsEnabled) return;
        
        // Define capsule start and end points
        Vector3 point1 = transform.position;
        Vector3 point2 = transform.position + Vector3.up * _collisionHeight;

        // Visualize the stationary capsule
        Gizmos.color = _gizmosColorActual;
        
        Gizmos.DrawWireSphere(point1, _collisionRadius);
        Gizmos.DrawWireSphere(point2, _collisionRadius);
        Gizmos.DrawLine(point1 + Vector3.right * _collisionRadius, point2 + Vector3.right * _collisionRadius);
        Gizmos.DrawLine(point1 - Vector3.right * _collisionRadius, point2 - Vector3.right * _collisionRadius);
        Gizmos.DrawLine(point1 + Vector3.forward * _collisionRadius, point2 + Vector3.forward * _collisionRadius);
        Gizmos.DrawLine(point1 - Vector3.forward * _collisionRadius, point2 - Vector3.forward * _collisionRadius);

        // Visualize the capsule cast
        Vector3 castPoint1 = point1 + _moveDirection.normalized * 0.25f;
        Vector3 castPoint2 = point2 + _moveDirection.normalized * 0.25f;
        Gizmos.color = _gizmosColorPredicted;
        
        Gizmos.DrawWireSphere(castPoint1, _collisionRadius);
        Gizmos.DrawWireSphere(castPoint2, _collisionRadius);
        Gizmos.DrawLine(castPoint1 + Vector3.right * _collisionRadius, castPoint2 + Vector3.right * _collisionRadius);
        Gizmos.DrawLine(castPoint1 - Vector3.right * _collisionRadius, castPoint2 - Vector3.right * _collisionRadius);
        Gizmos.DrawLine(castPoint1 + Vector3.forward * _collisionRadius, castPoint2 + Vector3.forward * _collisionRadius);
        Gizmos.DrawLine(castPoint1 - Vector3.forward * _collisionRadius, castPoint2 - Vector3.forward * _collisionRadius);
    }
    #endregion


    #region MovementLogic
    private void MovePlayer() {
        
        _transform.position += _moveDirection * Time.deltaTime * _speed;
        _transformCamera.position += _moveDirection * Time.deltaTime * _speed;
    }
    #endregion

    
    #region CollisionDetection
    // NOTE (SAVIZ): It is best to avoid colliders for the floor, to prevent collision when player is placed on (Y = 0.0f).
    // NOTE (SAVIZ): It is best to use 'CapsuleCast' shape for checking collision. Even if visual model is not 100% the same.
    private bool willCollide() {
        
        // TODO (SAVIZ): Make a custom field for controlling the maxDistance.
        return (Physics.CapsuleCast(_transform.position, _transform.position + (Vector3.up * _collisionHeight), _collisionRadius, _moveDirection, 0.25f));
    }    
    #endregion
}
