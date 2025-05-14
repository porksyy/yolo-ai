using System;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Data to be saved
    public DateTime playerStartTime;
    public int ageGroupIndex;

    public int dailyFeedCount;
    public int poopCounter;
    public float timeSinceLastFeed;
    public float timeSinceLastSleep;
    public float awakeTime;
    public float sleepTime;
    public float timeSinceLastPoop;

    public bool isSleeping;
    public bool hasPooped;
    public float poopTimer;

    

    public int tick;

    public GameData()
    {
        this.playerStartTime = DateTime.Now;
        this.ageGroupIndex = 0;
        this.dailyFeedCount = 0;
        this.poopCounter = 0;
        this.timeSinceLastFeed = 0;
        this.timeSinceLastSleep = 0;
        this.awakeTime = 0;
        this.sleepTime = 0;
        this.timeSinceLastPoop = 0;
        this.isSleeping = false;
        this.hasPooped = false;
        this.poopTimer = 0;
    }
}
