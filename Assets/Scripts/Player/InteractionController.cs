using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour {
    
    [Header("Dependencies")]
    [Tooltip("The selected 'Transform' placed in this field will be used as origin detection point.")]
    [SerializeField] private Transform _transform = null;

    [Header("Events")]
    [Space(10)]
    public UnityEvent<InteractionController> OnFoundInteractableChanged;
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls the 'Enabled' state of the Gizmos.")]
    [SerializeField] private bool _gizmosIsEnabled = true;
    
    [Tooltip("Controls the color of the drawn Gizmos.")]
    [SerializeField] private Color _gizmosColor = Color.red;
    
    [Space(10)]
    [Tooltip("Controls the range within which the player can detect and interact with an interactable object. (Also used for drawing the Gizmos)")]
    [SerializeField] private float _interactionRange = 5.0f;
    
    private IInteractable _foundInteractable = null;
    
    private IInteractable _interactedInteractable = null;
    
    private bool _keyIsDownInteraction = false;
    
    private void Update() {
        RaycastHit? raycastHit = TryObtainHit();
        
        _foundInteractable = TryObtainInteractable(raycastHit);

        OnFoundInteractableChanged.Invoke(this);
        
        bool canInteract = _foundInteractable != null && _keyIsDownInteraction;
        
        if (canInteract) {
            _interactedInteractable = _foundInteractable;
            _interactedInteractable.Interact(gameObject);
        }
    }
    
    private void OnDrawGizmos() {
        if (!_gizmosIsEnabled) {
            return;
        }

        Gizmos.color = _gizmosColor;
        Gizmos.DrawRay(_transform.position, _transform.forward * _interactionRange);
    }
    
    // Invoked by 'PlayerInput':
    public void OnInteracted(InputAction.CallbackContext context) {
        _keyIsDownInteraction = context.ReadValueAsButton();
    }
    
    public IInteractable GetFoundInteractable() {
        return (_foundInteractable);
    }
    
    private RaycastHit? TryObtainHit() {
        bool success = Physics.Raycast(this._transform.position, this._transform.forward, out RaycastHit hit, this._interactionRange);

        if (!success) {
            return (null);
        }

        return (hit);
    }

    private IInteractable TryObtainInteractable(RaycastHit? hit) {
        if (!hit.HasValue) {
            return (null);
        }

        hit.Value.collider.TryGetComponent(out IInteractable interactable);

        if (interactable == null) {
            return (null);
        }

        return (interactable);
    }
}
