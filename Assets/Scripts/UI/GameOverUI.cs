using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    private Label _labelInteractionMessage = null;

    
    private void Awake() {
        _labelInteractionMessage = _uiDocument.rootVisualElement.Q<Label>("Label_GameOver");
        
        _labelInteractionMessage.text = "5";
        
        HideUI();
    }

    private void Update() {
        if (!GameManager.Instance.IsGameOver()) {

            return;
        }
        
        ShowUI();
        _labelInteractionMessage.text = "GAME OVER" + "\n" + "YOU WIN!";
    }

    private void ShowUI() {
        _uiDocument.rootVisualElement.visible = true;
    }

    private void HideUI() {
        _uiDocument.rootVisualElement.visible = false;
    }
}
