using UnityEngine;

public interface IInteractable {

    // The 'Interact()' method may require access to various components or data due to the wide range of actions a script can perform. 
    // To simplify this, it is common to pass the entire 'GameObject', as a parameter, allowing the script to locate and access the necessary 
    // components or information directly from the interactor.
    void Interact(GameObject interactorObject);
    
    // A message interaction is quite standard. No need for additional information.
    string ObtainInteractionText();
    
    // To enable obtaining the 'Transform' of the interactable object.
    Transform ObtainTransform();
}
