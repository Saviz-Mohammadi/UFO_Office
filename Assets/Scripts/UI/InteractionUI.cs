using System;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionUI : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    private Label _labelInteractionMessage = null;

    
    private void OnEnable() {
        _labelInteractionMessage = _uiDocument.rootVisualElement.Q<Label>("InteractionLabel");
        
        _labelInteractionMessage.text = "";
    }
    
    public void OnFoundInteractableChanged(InteractionController interactionController) {
        IInteractable interactable = interactionController.GetFoundInteractable();

        if (interactable != null) {
            _labelInteractionMessage.text = interactable.GetInteractionText();
            ShowUI();
            
            return;
        }
        
        HideUI();
    }

    private void ShowUI() {
        _uiDocument.rootVisualElement.visible = true;
    }

    private void HideUI() {
        _uiDocument.rootVisualElement.visible = false;
    }
}
