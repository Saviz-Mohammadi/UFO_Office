using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour {
    
    [Header("Dependencies")]
    [Tooltip("The selected 'Transform' placed in this field will be used as origin detection point.")]
    [SerializeField] private Transform _transform = null;
    
    
    // Used to store the most recent interactable object:
    private IInteractable _foundInteractable = null;

    public IInteractable FoundInteractable => _foundInteractable;


    [Header("Fields (Customizable)")]
    [Tooltip("Controls the 'Enabled' state of the Gizmos.")]
    [SerializeField] private bool _gizmosIsEnabled = true;
    
    [Tooltip("Controls the color of the drawn Gizmos.")]
    [SerializeField] private Color _gizmosColor = Color.red;
    
    [Space(10)]
    [Tooltip("Controls the range within which the player can detect and interact with an interactable object. (Also used for drawing the Gizmos)")]
    [SerializeField] private float _interactionRange = 5.0f;

    
    
    [Space(10)]
    // Event is fired when this script discovers an interactable object: 
    public UnityEvent<InteractionController> OnInteractableDiscovered;
    
    // Event is fired when the last interactable object was either replaced with a new one or just lost:
    public UnityEvent<InteractionController> OnInteractableDiscarded;
    
    
    
    private void Update() {
        
        // Continuously scan the environment for new interactable objects:
        _foundInteractable = ObtainInteractableObject();
        
        // Invoke Unity events and allow other components to know what happened:
        if (_foundInteractable != null) {
            
            OnInteractableDiscovered?.Invoke(this);
        }

        else {
            OnInteractableDiscarded?.Invoke(this);
        }
    }
    
    private void OnDrawGizmos() {

        if (!_gizmosIsEnabled) {
            return;
        }
        
        Gizmos.color = _gizmosColor;
        
        Gizmos.DrawRay(_transform.position, _transform.forward * _interactionRange);
    }
    
    // Called on 'Interacted' Unity-Event of 'Player Input' component:
    public void OnInteracted(InputAction.CallbackContext context) {

        if (context.performed) {
         
            if (_foundInteractable != null) {
            
                _foundInteractable.Interact(gameObject);
            }
        }
    }
    
    public IInteractable GetInteractable() {
     
        return (_foundInteractable);
    }
    
    private IInteractable ObtainInteractableObject() {
        
        bool success = Physics.Raycast(_transform.position, _transform.forward, out RaycastHit hit, _interactionRange);

        if (!success) {
            return (null);
        }
        
        hit.collider.TryGetComponent(out IInteractable interactable);
            
        if (interactable == null) {
            return (null);
        }
            
        return (interactable);
    }
}
