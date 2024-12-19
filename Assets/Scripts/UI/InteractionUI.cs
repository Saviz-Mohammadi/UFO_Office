using System;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionUI : MonoBehaviour
{
    #region Fields
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    [Tooltip("Required for responding to interactions.")]
    [SerializeField] private InteractionController _interactionController = null;
    
    
    
    [Header("Fields (Read-only)")]
    [Tooltip("Represents the 'Label' element responsible for displaying the interaction message.")]
    [SerializeField] private Label _labelInteractionMessage = null;
    #endregion

    
    #region Unity Events
    private void OnEnable() {
        
        _labelInteractionMessage = _uiDocument.rootVisualElement.Q<Label>("InteractionLabel");
        _labelInteractionMessage.text = "";
    }
    #endregion

    public void OnInteractableFound() {
        
        ShowUI();
        _labelInteractionMessage.text = _interactionController.GetInteractable().ObtainInteractionText();
    }

    public void OnInteractableLost() {
        
        HideUI();
        //_labelInteractionMessage.text = _interactionController.GetInteractable().ObtainInteractionText();
    }

    private void ShowUI() {
        
        _uiDocument.rootVisualElement.visible = true;
    }

    private void HideUI() {
        
        _uiDocument.rootVisualElement.visible = false;
    }
}
