using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuUI : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    [SerializeField] private OptionsMenuUI _optionsMenuUI = null;
    
    private Button _buttonResume = null;
    private Button _buttonOptions = null;
    private Button _buttonQuit = null;
    
    public bool optionsMenuOpen = false;
    
    private void Awake() {
        _buttonResume = _uiDocument.rootVisualElement.Q<Button>("Button_Resume");
        _buttonOptions = _uiDocument.rootVisualElement.Q<Button>("Button_Options");
        _buttonQuit = _uiDocument.rootVisualElement.Q<Button>("Button_LoadMainMenu");
        
        _buttonResume.clicked += OnResumeClicked;
        _buttonOptions.clicked += OnOptionsClicked;
        _buttonQuit.clicked += OnQuitClicked;
    }

    private void Update() {
        if (GameManager.Instance.isPaused && !optionsMenuOpen) {
            ShowUI();
            return;
        }
        
        HideUI();
    }

    private void OnResumeClicked() {
        GameManager.Instance.TogglePause();
    }

    private void OnOptionsClicked() {
        optionsMenuOpen = true;
        _optionsMenuUI.ShowUI();
        HideUI();
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
