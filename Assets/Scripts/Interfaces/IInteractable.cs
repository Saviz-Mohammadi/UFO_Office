using UnityEngine;
using UnityEngine.Events;

public interface IInteractable {
    /// <summary>
    /// This method is used to react to the 'OnFoundInteractableChanged' event of the 'InteractionController'. Usually, used to just to provide visual hints to the player. 
    /// </summary>
    /// <param name="interactionController"></param>
    public void OnFoundInteractableChanged(InteractionController interactionController);
    
    /// <summary>
    /// Event used to notify other objects that the interaction is completed.
    /// </summary>
    public UnityEvent OnInteracted { get; }
    
    /// <summary>
    /// This method enables the interaction process.
    /// </summary>
    /// <param name="interactorObject"></param>
    public void Interact(GameObject interactorObject);

    /// <summary>
    /// This method enables to visually locate the interactable.
    /// </summary>
    /// <param name="state"></param>
    public void SetLocatorState(bool state);
    
    /// <summary>
    /// This method enables the retrieval of a text interaction string.
    /// </summary>
    /// <returns>string</returns>
    public string GetInteractionText();
}
