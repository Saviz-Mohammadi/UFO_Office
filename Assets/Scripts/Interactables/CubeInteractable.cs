using UnityEngine;

public class CubeInteractable : MonoBehaviour, IInteractable {
    
    [Header("Dependencies")]
    [Tooltip("The selected 'MeshRenderer' placed in this field will be used to target material changes.")]
    [SerializeField] private MeshRenderer _meshRenderer;
    
    [Tooltip("The selected 'Material' placed in this field will be used as discovered indication.")]
    [SerializeField] private Material _materialDiscovered;
    
    [Tooltip("The selected 'Material' placed in this field will be used as discarded indication. (Usually the same as original material of the object)")]
    [SerializeField] private Material _materialDiscarded;
    
    public void Interact(GameObject interactorObject) {
        
        Debug.Log("You interacted with me: " + gameObject.name);
    }

    public void OnInteractableDiscovered(InteractionController interactionController) {

        if (ReferenceEquals(this, interactionController.FoundInteractable)) {
            OnDiscovered();

            return;
        }
        
        OnDiscarded();
    }
    
    public void OnInteractableDiscarded(InteractionController interactionController) {
        
        OnDiscarded();
    }

    public string ObtainInteractionText() {

        return ("My name is: " + gameObject.name);
    }

    public void OnDiscovered() {

        _meshRenderer.material = _materialDiscovered;
    }

    public void OnDiscarded() {
        
        _meshRenderer.material = _materialDiscarded;
    }

    public GameObject ObtainGameObject() {

        return (gameObject);
    }
}
