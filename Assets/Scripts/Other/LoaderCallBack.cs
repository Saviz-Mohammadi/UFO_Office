using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LoaderCallBack : MonoBehaviour {
    [Header("Dependencies")]
    [Tooltip("Required for interacting with UI elements.")]
    [SerializeField] private UIDocument _uiDocument = null;
    
    private ProgressBar _progressBar = null;
    
    private void Awake() {
        _progressBar = _uiDocument.rootVisualElement.Q<ProgressBar>("ProgressBar_LoadStatus");
    }

    private void Start() {
        Loader.LoadScene("OfficeScene", _progressBar);
    }
}
