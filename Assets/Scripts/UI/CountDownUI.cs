using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CountDownUI : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    private Label _labelInteractionMessage = null;

    
    private void Awake() {
        _labelInteractionMessage = _uiDocument.rootVisualElement.Q<Label>("Label_CountDown");
        
        HideUI();
    }

    private void Update() {
        if (!GameManager.Instance.IsCountingDown()) {
            HideUI();

            return;
        }
        
        ShowUI();
        var (h, m, s) = GameManager.Instance.GetTickTime();
        _labelInteractionMessage.text = ConvertToTimeString(h, m, s);
    }

    private string ConvertToTimeString(int hours, int minutes, int seconds) {
        return ($"{seconds}");
    }
    
    private void ShowUI() {
        _uiDocument.rootVisualElement.visible = true;
    }

    private void HideUI() {
        _uiDocument.rootVisualElement.visible = false;
    }
}
