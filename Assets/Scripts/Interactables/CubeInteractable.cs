using UnityEngine;

public class CubeInteractable : MonoBehaviour, IInteractable {
    [Header("Dependencies")]
    [Tooltip("The selected 'MeshRenderer' placed in this field will be used to target material changes.")]
    [SerializeField] private MeshRenderer _meshRenderer;
    
    [Tooltip("The selected 'Material' placed in this field will be used as discovered indication.")]
    [SerializeField] private Material _materialDiscovered;
    
    [Tooltip("The selected 'Material' placed in this field will be used as discarded indication. (Usually, the same as original material of the object)")]
    [SerializeField] private Material _materialDiscarded;

    public void OnFoundInteractableChanged(InteractionController interactionController) {
        IInteractable interactable = interactionController.GetFoundInteractable();

        if (interactable != null && ReferenceEquals(this, interactable)) {
            ShowHint();
            return;
        }
        
        HideHint();
    }
    
    public void Interact(GameObject interactorObject) {
        Debug.Log("You interacted with me: " + gameObject.name);
    }
    
    public string GetInteractionText() {
        return ("My name is: " + gameObject.name);
    }

    private void ShowHint() {
        _meshRenderer.material = _materialDiscovered;
    }

    private void HideHint() {
        _meshRenderer.material = _materialDiscarded;
    }
}