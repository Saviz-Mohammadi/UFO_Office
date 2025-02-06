using System;
using UnityEngine;

public class StopwatchTimer : MonoBehaviour
{
    private float elapsedTime;
    private bool isRunning;

    public event Action<int, int, int> OnTimeUpdated; // Returns hours, minutes, seconds

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        StopTimer();
        elapsedTime = 0;
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime; // Accumulate elapsed time

        var (h, m, s) = ConvertFromSeconds(elapsedTime);
        OnTimeUpdated?.Invoke(h, m, s);
    }

    public (int hours, int minutes, int seconds) GetElapsedTime()
    {
        return ConvertFromSeconds(elapsedTime);
    }

    private (int hours, int minutes, int seconds) ConvertFromSeconds(float totalSeconds)
    {
        int h = Mathf.FloorToInt(totalSeconds / 3600);
        int m = Mathf.FloorToInt((totalSeconds % 3600) / 60);
        int s = Mathf.FloorToInt(totalSeconds % 60);
        return (h, m, s);
    }
}