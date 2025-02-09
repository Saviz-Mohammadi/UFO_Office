using UnityEngine;
using UnityEngine.UIElements;

public class OptionsMenuUI : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    [SerializeField] private PauseMenuUI _pauseMenuUI = null;
    
    // TODO (SAVIZ): We desperately need to reafctor this to rely on events instead of polling via update()
    
    private Slider _sliderMusic = null;
    private Slider _sliderSFX = null;
    private Button _buttonBack = null;
    
    private void Awake() {
        _sliderMusic = _uiDocument.rootVisualElement.Q<Slider>("Slider_Music");
        _sliderSFX = _uiDocument.rootVisualElement.Q<Slider>("Slider_SFX");
        _buttonBack = _uiDocument.rootVisualElement.Q<Button>("Button_Back");

        _sliderMusic.RegisterValueChangedCallback(OnSliderMusicValueChanged);
        _sliderSFX.RegisterValueChangedCallback(OnSliderSFXValueChanged);
        _buttonBack.clicked += OnBackClicked;
        
        HideUI();
    }
    
    private void Update() {
        if (GameManager.Instance.isPaused) {
            return;
        }
        
        HideUI();
    }

    private void OnSliderMusicValueChanged(ChangeEvent<float> evt) {
        AudioManager.Instance.ChangedMusicVolume(evt.newValue);
    }
    
    private void OnSliderSFXValueChanged(ChangeEvent<float> evt) {
        AudioManager.Instance.ChangedSfxVolume(evt.newValue);
    }
    
    private void OnBackClicked() {
        HideUI();
    }
    
    public void ShowUI() {
        _uiDocument.rootVisualElement.visible = true;
    }

    private void HideUI() {
        _uiDocument.rootVisualElement.visible = false;
        _pauseMenuUI.optionsMenuOpen = false;
    }
}
