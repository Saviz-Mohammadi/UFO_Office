using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ClockUI : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    private Label _labelInteractionMessage = null;

    
    private void Awake() {
        _labelInteractionMessage = _uiDocument.rootVisualElement.Q<Label>("Label_Clock");
        
        HideUI();
    }

    // private void Start() {
    //     _labelInteractionMessage.text = ConvertToTimeString(GameManager.Instance.GetPlayTimeMax());
    // }

    private void Update() {
        if (!GameManager.Instance.IsPlaying()) {

            return;
        }
        
        ShowUI();
        var (h, m, s) = GameManager.Instance.GetTickTime();
        _labelInteractionMessage.text = ConvertToTimeString(h, m, s);
    }

    private string ConvertToTimeString(int hours, int minutes, int seconds) {
        return ($"{hours:D2}:{minutes:D2}:{seconds:D2}");
    }
    
    private void ShowUI() {
        _uiDocument.rootVisualElement.visible = true;
    }

    private void HideUI() {
        _uiDocument.rootVisualElement.visible = false;
    }
}
