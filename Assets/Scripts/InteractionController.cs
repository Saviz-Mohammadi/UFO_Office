using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

public class InteractionController : MonoBehaviour {

    // TODO (SAVIZ): Make Gizmos for this.
    
    #region Fields
    [Header("Dependencies")]
    [Tooltip("The selected 'Transform' placed in this field will be used as origin detection point.")]
    [SerializeField] private Transform _transform = null;
    
    
    
    [Header("Fields (Read Only)")]
    [Tooltip("Represents the found IInteractable object in the scene.")]
    [SerializeField] private IInteractable _foundInteractable = null;
    
    
    
    [Header("Fields (Customizable)")]
    [Tooltip("Controls the range within which the player can detect and interact with an interactable object.")]
    [SerializeField] private float _interactionRange = 3.0f;
    #endregion


    #region Unity Events
    private void Update() {
        
        // Continuously scan the environment for new interactable objects:
        _foundInteractable = ObtainInteractableObjects();
        
        // Invoke Unity events and allow other scripts to know what happened:
        if (_foundInteractable != null) {
            
            onInteractableFound?.Invoke();
        }

        else {
            onInteractableLost?.Invoke();
        }
    }
    #endregion


    #region Event Related Logic
    
    public UnityEvent onInteractableFound;
    public UnityEvent onInteractableLost;
    
    // This method will be invoked as the result of the "onInteraction()" event of "InputController".
    // The main task is to respond to the interaction click of the player and see if interaction is possible.
    public void Interact() {
        
        if (_foundInteractable != null) {
            
            _foundInteractable.Interact(gameObject);
        }
    }
    
    // I know this is not technically event-related, but currently I have no idea how to make an event pass my 'IInteractable' as an argument to the listeting scripts. So, I just made this method to access the field directly:
    public IInteractable GetInteractable() {
     
        return _foundInteractable;
    }
    
    #endregion

    
    #region ScanEnvironment
    private IInteractable ObtainInteractableObjects() {

        List<IInteractable> interactables = new List<IInteractable>();
        
        Collider[] colliders = Physics.OverlapSphere(_transform.position, _interactionRange);
        
        foreach (Collider collider in colliders) {
            
            if (collider.TryGetComponent(out IInteractable interactable)) {
                
                interactables.Add(interactable);
            }
        }

        IInteractable existingInteractable = null;
        
        // Check which interactable object is closer to player:
        foreach (IInteractable interactable in interactables) {

            if (existingInteractable == null) {
                
                existingInteractable = interactable;
            }

            else {
                float newInteractableDistance = Vector3.Distance(_transform.position, interactable.ObtainTransform().position);
                float existingInteractableDistance = Vector3.Distance(_transform.position, existingInteractable.ObtainTransform().position);

                if (newInteractableDistance < existingInteractableDistance) {
                    
                    existingInteractable = interactable;
                }
            }
        }
        
        return existingInteractable;
    }
    #endregion
}
