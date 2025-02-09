using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    private Button _buttonPlay = null;
    private Button _buttonQuit = null;
    
    private void Awake() {
        _buttonPlay = _uiDocument.rootVisualElement.Q<Button>("Button_Play");
        _buttonQuit = _uiDocument.rootVisualElement.Q<Button>("Button_Quit");
        
        _buttonPlay.clicked += OnPlayClicked;
        _buttonQuit.clicked += OnQuitClicked;
    }

    private void OnPlayClicked() {
        Loader.LoadScene("LoadingScene");
    }

    private void OnQuitClicked() {
        Application.Quit();
    }
}
