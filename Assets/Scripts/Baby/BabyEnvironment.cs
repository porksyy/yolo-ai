using UnityEngine;
using System.Collections.Generic;

public class BabyEnvironment : MonoBehaviour
{
    private List<float> feedingTimes = new List<float>();
    private List<float> sleepTimes = new List<float>();

    public float feedingWindow = 1f; // Acceptable range to match pattern
    public float sleepingWindow = 1f;

    public void RecordFeedingTime(float time)
    {
        feedingTimes.Add(time);
        if (feedingTimes.Count > 5)
        {
            feedingTimes.RemoveAt(0); // Maintain a short history
        }
    }

    public void ResetFeedingHistory()
    {
        feedingTimes.Clear();
    }

    public void RecordSleepTime(float time)
    {
        sleepTimes.Add(time);
        if(sleepTimes.Count > 5)
        {
            sleepTimes.RemoveAt(0); // Maintain a short history
        }
    }

    public void ResetSleepingHistory()
    {
        sleepTimes.Clear();
    }

    // Determine if baby's current time is near its learned feeding pattern
    public bool IsFeedingTime(float currentTime)
    {
        if (feedingTimes.Count == 0) return false;

        float averageTime = 0f;
        foreach (float t in feedingTimes)
        {
            averageTime += t;
        }
        averageTime /= feedingTimes.Count;

        return Mathf.Abs(currentTime - averageTime) < feedingWindow;
    }

    public bool IsSleepingTime(float currentTime)
    {
        if (sleepTimes.Count == 0) return false;
        float averageTime = 0f;
        foreach (float t in sleepTimes)
        {
            averageTime += t;
        }
        averageTime /= sleepTimes.Count;
        return Mathf.Abs(currentTime - averageTime) < sleepingWindow;
    }
}