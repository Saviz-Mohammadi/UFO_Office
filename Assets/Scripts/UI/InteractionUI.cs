using System;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionUI : MonoBehaviour {

    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    
    [Header("Fields (Read-only)")]
    [Tooltip("Represents the 'Label' element responsible for displaying the interaction message.")]
    [SerializeField] private Label _labelInteractionMessage = null;

    
    private void OnEnable() {
        
        _labelInteractionMessage = _uiDocument.rootVisualElement.Q<Label>("InteractionLabel");
        
        _labelInteractionMessage.text = "";
    }

    // Called on 'OnInteractableDiscovered' Unity-Event of 'InteractionController' component:
    public void OnInteractableDiscovered(InteractionController interactionController) {
        
        _labelInteractionMessage.text = interactionController.GetInteractable().ObtainInteractionText();
        
        ShowUI();
    }

    // Called on 'OnInteractableDiscarded' Unity-Event of 'InteractionController' component:
    public void OnInteractableDiscarded(InteractionController interactionController) {
        
        HideUI();
    }

    private void ShowUI() {
        
        _uiDocument.rootVisualElement.visible = true;
    }

    private void HideUI() {
        
        _uiDocument.rootVisualElement.visible = false;
    }
}
