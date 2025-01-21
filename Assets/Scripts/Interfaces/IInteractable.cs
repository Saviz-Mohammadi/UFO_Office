using UnityEngine;

public interface IInteractable {

    // To enable the 'Interact()' method to access anything it may need from the interactor.
    public void Interact(GameObject interactorObject);
    
    // A Standard interaction message. A simple nice touch.
    public string ObtainInteractionText();
    
    // This enables a change in the interactable object when it is discovered.
    public void OnDiscovered();

    // This enables a change in the interactable object when it is discarded.
    public void OnDiscarded();
    
    // This enables us to access anything we need from the interactable object:
    public GameObject ObtainGameObject();
}
