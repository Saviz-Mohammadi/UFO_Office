using System;
using UnityEngine;
using UnityEngine.Events;

public class CubeInteractable : MonoBehaviour, IInteractable {
    [Header("Dependencies")]
    [Tooltip("The selected 'MeshRenderer' placed in this field will be used to target material changes.")]
    [SerializeField] private MeshRenderer _meshRenderer;
    
    [Tooltip("Used for playing a sound in 3D when object is interacted with. (The settings for the 'AudioSource' will be obtained from universal 'AudioManager')")]
    [SerializeField] private AudioSource _audioSource;
    
    [Tooltip("The selected 'Material' placed in this field will be used as discovered indication.")]
    [SerializeField] private Material _materialDiscovered;
    
    [Tooltip("The selected 'Material' placed in this field will be used as discarded indication. (Usually, the same as original material of the object)")]
    [SerializeField] private Material _materialDiscarded;

    private void Awake() {
        TaskManager.Instance.AddTask("The cube", this);
        CanInteract = true;
    }

    public bool CanInteract { get; set; }

    public void OnFoundInteractableChanged(InteractionController interactionController) {
        IInteractable interactable = interactionController.GetFoundInteractable();

        if (interactable != null && ReferenceEquals(this, interactable)) {
            ShowHint();
            return;
        }
        
        HideHint();
    }

    public UnityEvent OnInteracted { get; }

    public void Interact(GameObject interactorObject) {
        if (!CanInteract) {
            return;
        }
        
        Debug.Log("You interacted with me: " + gameObject.name);
        _audioSource.Play();
        
        OnInteracted?.Invoke();
        
        CanInteract = false; // Usually, the interaction must be enabled from other modules such as 'TaskManager'.
    }

    public void SetLocatorState(bool state) {
        // Display a flashing pink arrow that rotates above the interactebl.
    }

    public string GetInteractionText() {
        return ("PRESS 'E' TO INTERACT WITH " + gameObject.name);
    }

    private void ShowHint() {
        _meshRenderer.material = _materialDiscovered;
    }

    private void HideHint() {
        _meshRenderer.material = _materialDiscarded;
    }
}