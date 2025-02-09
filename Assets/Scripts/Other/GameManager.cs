using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private CountdownTimer _countdownTimer;
    
    public float audioVolume = 1.0f;
    public int graphicsQuality = 2; // 0 = Low, 1 = Medium, 2 = High
    public bool isPaused = false;
    public int targetFPS = 60; // Default FPS setting
    
    private enum State {
        WaitingToStart,
        CountDown,
        Playing,
        Over
    }
    
    private State state = State.WaitingToStart;
    
    // These are options:
    private float waitTime = 1.0f;
    private float countDownTime = 5.0f;
    private float playTime = 10.0f;
    
    private int seconds = 0;
    private int minutes = 0;
    private int hours = 0;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start() {
        // Subscribe to event:
        _countdownTimer.OnCountdownEnd += OnCountDownEnd;
        _countdownTimer.OnTimeTicked += OnTimeTick;
        
        // Start with waitTime:
        _countdownTimer.Initialize(0, 0, 1);
        _countdownTimer.StartTimer();
    }
    
    public void OnCountDownEnd() {
        switch (state) {
            case State.WaitingToStart:
                _countdownTimer.StopTimer();
                _countdownTimer.Initialize(0, 0, 5);
                state = State.CountDown;
                _countdownTimer.StartTimer();
                break;
            case State.CountDown:
                SetCursorState(false, true);
                _countdownTimer.StopTimer();
                _countdownTimer.Initialize(0, 2, 30);
                state = State.Playing;
                _countdownTimer.StartTimer();
                break;
            case State.Playing:
                state = State.Over;
                break;
        }
    }

    public void OnTimeTick(int hour, int minute, int second) {
        seconds = second;
        minutes = minute;
        hours = hour;
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

    public (int, int, int) GetTickTime() {
        return (hours, minutes, seconds);
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
        // NOTE (SAVIZ): In an offline context, it would make sense to set the Time.scaleFactor to 0, however, since this is a multiplayer game it would be wierd and not perferrable to do that.
        isPaused = !isPaused;
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