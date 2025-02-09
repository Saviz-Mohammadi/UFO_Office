using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuUI : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    private Button _buttonResume = null;
    private Button _buttonQuit = null;
    
    private void Awake() {
        _buttonResume = _uiDocument.rootVisualElement.Q<Button>("Button_Resume");
        _buttonQuit = _uiDocument.rootVisualElement.Q<Button>("Button_LoadMainMenu");
        
        _buttonResume.clicked += OnResumeClicked;
        _buttonQuit.clicked += OnQuitClicked;
    }

    private void Update() {
        if (GameManager.Instance.isPaused) {
            ShowUI();
            return;
        }
        
        HideUI();
    }

    private void OnResumeClicked() {
        GameManager.Instance.TogglePause();
    }

    private void OnQuitClicked() {
        Loader.LoadScene("MainMenuScene");
    }
    
    private void ShowUI() {
        _uiDocument.rootVisualElement.visible = true;
    }

    private void HideUI() {
        _uiDocument.rootVisualElement.visible = false;
    }
}
