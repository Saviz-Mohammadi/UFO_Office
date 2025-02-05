using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float audioVolume = 1.0f;
    public int graphicsQuality = 2; // 0 = Low, 1 = Medium, 2 = High
    public bool isPaused = false;
    public GameObject pauseMenuUI;
    public int targetFPS = 60; // Default FPS setting

    private enum State {
        WaitingToStart,
        CountDown,
        Playing,
        Over
    }
    
    private State state = State.WaitingToStart;
    private float waitTime = 1.0f;
    private float countDownTime = 5.0f;
    private float playTimeMax = 10.0f;
    private float playTime;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                waitTime -= Time.deltaTime;
                if (waitTime < 0.0f) {
                    state = State.CountDown;
                }
                break;
            case State.CountDown:
                countDownTime -= Time.deltaTime;
                if (countDownTime < 0.0f) {
                    playTime = playTimeMax;
                    state = State.Playing;
                }
                break;
            case State.Playing:
                playTime -= Time.deltaTime;
                if (playTime < 0.0f) {
                    playTime = Mathf.Max(playTime, 0); // Ensure it doesn't go negative
                    state = State.Over;
                }
                break;
        }
    }

    public bool IsPlaying() {
        return (state == State.Playing);
    }

    public bool IsCountingDown() {
        return (state == State.CountDown);
    }

    public bool IsGameOver() {
        return (state == State.Over);
    }

    public float GetCountDownTime() {
        return (countDownTime);
    }

    public float GetPlayTimeMax() {
        return (playTimeMax);
    }

    public float GetPlayTime() {
        return playTime;
    }
    
    public void SetAudioVolume(float volume)
    {
        audioVolume = volume;
        PlayerPrefs.SetFloat("AudioVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetGraphicsQuality(int quality)
    {
        graphicsQuality = quality;
        QualitySettings.SetQualityLevel(quality);
        PlayerPrefs.SetInt("GraphicsQuality", quality);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("AudioVolume"))
        {
            audioVolume = PlayerPrefs.GetFloat("AudioVolume");
        }

        if (PlayerPrefs.HasKey("GraphicsQuality"))
        {
            graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality");
            QualitySettings.SetQualityLevel(graphicsQuality);
        }
    }
    
    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        // Automatically update cursor state based on pause state
        SetCursorState(isPaused, !isPaused);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void SetCursorState(bool visible, bool locked)
    {
        Cursor.visible = visible;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }
    
    public void ApplyFrameRateSettings()
    {
        QualitySettings.vSyncCount = 0; // Disable VSync (use manual FPS control)
        Application.targetFrameRate = targetFPS; // Set desired FPS
    }

    public void SetTargetFPS(int fps)
    {
        targetFPS = fps;
        Application.targetFrameRate = fps;
    }
}