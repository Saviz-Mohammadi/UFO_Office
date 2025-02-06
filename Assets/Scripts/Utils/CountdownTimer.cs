using System;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    private float timeRemaining;
    private bool isRunning;

    public event Action OnCountdownEnd;
    public event Action<int, int, int> OnTimeTicked; // Returns hours, minutes, seconds

    public void Initialize(int hours, int minutes, int seconds)
    {
        timeRemaining = ConvertToSeconds(hours, minutes, seconds);
        isRunning = false;
    }

    public void StartTimer()
    {
        if (timeRemaining > 0)
            isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer(int hours, int minutes, int seconds)
    {
        StopTimer();
        timeRemaining = ConvertToSeconds(hours, minutes, seconds);
    }

    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
            OnCountdownEnd?.Invoke();
        }

        var (h, m, s) = ConvertFromSeconds(timeRemaining);
        OnTimeTicked?.Invoke(h, m, s);
    }

    public (int hours, int minutes, int seconds) GetRemainingTime()
    {
        return ConvertFromSeconds(timeRemaining);
    }

    private float ConvertToSeconds(int hours, int minutes, int seconds)
    {
        return (hours * 3600) + (minutes * 60) + seconds;
    }

    private (int hours, int minutes, int seconds) ConvertFromSeconds(float totalSeconds)
    {
        int h = Mathf.FloorToInt(totalSeconds / 3600);
        int m = Mathf.FloorToInt((totalSeconds % 3600) / 60);
        int s = Mathf.FloorToInt(totalSeconds % 60);
        return (h, m, s);
    }
}