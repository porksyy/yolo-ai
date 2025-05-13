using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Unity.VisualScripting;
using System;

public class BabyAgent : Agent, IDataPersistence
{
    public enum AgeGroup { Month0To3, Month4To6, Month6To12 }
    public AgeGroup ageGroup = AgeGroup.Month0To3;

    private Animator animator;

    public Caregiver caregiver;
    public bool userControlResponded = true;

    private int tick; // Each tick = 1 minute
    private int dailyFeedCount;
    private int poopCounter;

    private float timeSinceLastFeed;
    private float timeSinceLastSleep;
    private float awakeTime;
    private float sleepTime;
    private float timeSinceLastPoop;

    public bool isSleeping = false;
    public bool hasPooped = false;
    private float poopTimer = -1;

    // for metrics
    private int correctCries = 0;
    private int falseCries = 0;
    private int totalCries = 0;
    private int totalNoActions = 0;

    private float realTimeAccumulator = 0f;
    public float secondsPerSimulatedMinute = 1f;

    private bool hasCriedThisNeed = false;
    private bool autoWakeEnabled = true;
    private System.Random rng = new System.Random();

    public void LoadData(GameData data)
    {
        // Calculate days passed since the player started
        int daysPassed = (DateTime.Now - data.playerStartTime).Days;

        //Update ageGroup based on days passed
        if (daysPassed >= 0 && daysPassed < 30)
            this.ageGroup = AgeGroup.Month0To3;
        else if (daysPassed < 90)
            this.ageGroup = AgeGroup.Month4To6;
        else
            this.ageGroup = AgeGroup.Month6To12;

        this.dailyFeedCount = data.dailyFeedCount;
        this.poopCounter = data.poopCounter;
        this.timeSinceLastFeed = data.timeSinceLastFeed;
        this.timeSinceLastSleep = data.timeSinceLastSleep;
        this.awakeTime = data.awakeTime;
        this.sleepTime = data.sleepTime;
        this.timeSinceLastPoop = data.timeSinceLastPoop;
        this.isSleeping = data.isSleeping;
        this.hasPooped = data.hasPooped;
        this.poopTimer = data.poopTimer;

        this.tick = data.tick;

        Debug.Log($"Loaded ageGroup: {ageGroup} (Days since start: {daysPassed})");
    }

    public void SaveData(ref GameData data)
    {
        data.ageGroupIndex = (int)this.ageGroup;
        data.dailyFeedCount = this.dailyFeedCount;
        data.poopCounter = this.poopCounter;
        data.timeSinceLastFeed = this.timeSinceLastFeed;
        data.timeSinceLastSleep = this.timeSinceLastSleep;
        data.awakeTime = this.awakeTime;
        data.sleepTime = this.sleepTime;
        data.timeSinceLastPoop = this.timeSinceLastPoop;
        data.isSleeping = this.isSleeping;
        data.hasPooped = this.hasPooped;
        data.poopTimer = this.poopTimer;
        data.tick = this.tick;

    }

    public override void Initialize()
    {
        animator = GetComponent<Animator>();
    }
    public override void OnEpisodeBegin()
    {
        // for metrics
        Academy.Instance.StatsRecorder.Add("Baby/FeedCount", dailyFeedCount);
        Academy.Instance.StatsRecorder.Add("Baby/ExpectedFeeds", GetExpectedFeedCount());

        float sleepHours = sleepTime / 60f;
        Academy.Instance.StatsRecorder.Add("Baby/SleepHours", sleepHours);
        Academy.Instance.StatsRecorder.Add("Baby/SleepMin", GetExpectedSleepHours().x);
        Academy.Instance.StatsRecorder.Add("Baby/SleepMax", GetExpectedSleepHours().y);

        Academy.Instance.StatsRecorder.Add("Baby/PoopCount", poopCounter);

        tick = 0;
        dailyFeedCount = 0;
        poopCounter = 0;
        timeSinceLastFeed = 0;
        timeSinceLastSleep = 0;
        awakeTime = 0;
        sleepTime = 0;
        timeSinceLastPoop = 0;
        isSleeping = false;
        hasPooped = false;
        poopTimer = -1;
        hasCriedThisNeed = false;
        Debug.Log($"Age Group = {(int)ageGroup}");
        animator.SetInteger("ageGroup", (int)ageGroup);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((int)ageGroup);
        sensor.AddObservation(dailyFeedCount);
        sensor.AddObservation(timeSinceLastFeed);
        sensor.AddObservation(timeSinceLastSleep);
        sensor.AddObservation(awakeTime);
        sensor.AddObservation(sleepTime);
        sensor.AddObservation(timeSinceLastPoop);
        sensor.AddObservation(hasPooped ? 1f : 0f);
        sensor.AddObservation(isSleeping ? 1f : 0f);
        sensor.AddObservation(tick / 1440f); // normalized time of day
    }

    //private void Update()
    //{
    //    realTimeAccumulator += Time.deltaTime;
    //    while (realTimeAccumulator >= secondsPerSimulatedMinute)
    //    {
    //        RequestDecision(); // Trigger a new action decision
    //        realTimeAccumulator -= secondsPerSimulatedMinute;
    //    }
    //}


    public override void OnActionReceived(ActionBuffers actions)
    {
        realTimeAccumulator += Time.deltaTime;
        while (realTimeAccumulator >= secondsPerSimulatedMinute)
        {
            SimulateMinute(actions.DiscreteActions[0]);
            realTimeAccumulator -= secondsPerSimulatedMinute;
        }
    }

    private void EvaluateDailyNeeds()
    {
        float expectedFeeds = GetExpectedFeedCount();
        if (dailyFeedCount < expectedFeeds)
            AddReward(-1f);
        else if (dailyFeedCount <= expectedFeeds + 1)
            AddReward(+1f);

        float sleepHours = sleepTime / 60f;
        Vector2 sleepRange = GetExpectedSleepHours();
        if (sleepHours < sleepRange.x || sleepHours > sleepRange.y)
            AddReward(-1f);
        else
            AddReward(+1f);
    }

    public bool IsHungry()
    {
        return ageGroup switch
        {
            AgeGroup.Month0To3 => timeSinceLastFeed >= 90,
            AgeGroup.Month4To6 => timeSinceLastFeed >= 120,
            AgeGroup.Month6To12 => timeSinceLastFeed >= 180,
            _ => false,
        };
    }

    public bool IsSleepy()
    {
        return ageGroup switch
        {
            AgeGroup.Month0To3 => awakeTime >= 60,
            AgeGroup.Month4To6 => awakeTime >= 90,
            AgeGroup.Month6To12 => awakeTime >= 120,
            _ => false,
        };
    }

    private float GetPoopDelay()
    {
        return ageGroup switch
        {
            AgeGroup.Month0To3 => 30f,
            AgeGroup.Month4To6 => 45f,
            AgeGroup.Month6To12 => 60f,
            _ => 60f,
        };
    }

    private float GetExpectedFeedCount()
    {
        return ageGroup switch
        {
            AgeGroup.Month0To3 => 11f,
            AgeGroup.Month4To6 => 9f,
            AgeGroup.Month6To12 => 5f,
            _ => 0f,
        };
    }

    private Vector2 GetExpectedSleepHours()
    {
        return ageGroup switch
        {
            AgeGroup.Month0To3 => new Vector2(14f, 17f),
            AgeGroup.Month4To6 => new Vector2(12f, 16f),
            AgeGroup.Month6To12 => new Vector2(12f, 12f),
            _ => Vector2.zero,
        };
    }

    private int GetSleepDurationTarget()
    {
        return ageGroup switch
        {
            AgeGroup.Month0To3 => 90,
            AgeGroup.Month4To6 => 120,
            AgeGroup.Month6To12 => 120,
            _ => 90,
        };
    }

    public void Feed()
    {
        Debug.Log($"Feed: poopCounter={poopCounter}, poopTimer={poopTimer}");

        dailyFeedCount++;
        timeSinceLastFeed = 0;
        poopCounter++;
        hasCriedThisNeed = false;

        if (poopCounter >= 3)
        {
            poopTimer = 0;
            poopCounter = 0;
        }
        AddReward(+0.1f);
    }

    public void PutToSleep()
    {
        if (!isSleeping) 
        {
            isSleeping = true;
            awakeTime = 0;
            timeSinceLastSleep = 0;
            hasCriedThisNeed = false;
            AddReward(+0.1f);
        }
        
    }

    public void CleanUp()
    {
        hasPooped = false;
        timeSinceLastPoop = 0;
        hasCriedThisNeed = false;
        AddReward(+0.1f);
    }

    private void SimulateMinute(int action)
    {
        tick++;

        // Time progression
        if (isSleeping)
            sleepTime += 1;
        else
            awakeTime += 1;

        timeSinceLastFeed += 1;
        timeSinceLastSleep += 1;
        timeSinceLastPoop += 1;

        // Handle poop delay
        if (poopTimer >= 0)
        {

            poopTimer += 1;
            Debug.Log($"Poop Timer Progress: {poopTimer}/{GetPoopDelay()}");
            if (poopTimer >= GetPoopDelay() + rng.Next(-5, 5))
            {
                hasPooped = true;
                poopTimer = -1;
                Debug.Log("Baby has pooped!");
                hasCriedThisNeed = false;
            }
        }

        bool hungry = IsHungry();
        bool sleepy = IsSleepy();

        if (action == 0)
        {
            animator.SetBool("isCrying", false);
            // Do nothing
            //AddReward(-0.001f); // tiny penalty to encourage decision-making
            if (hungry || sleepy || hasPooped)
            {
                AddReward(-0.1f);

            }
            else
            {
                AddReward(+0.01f);
            }
            totalNoActions++;
            Debug.Log($"Age: {ageGroup}, IsHungry: {hungry}, IsSleepy: {sleepy}, Pooped : {hasPooped}, Cry: 1, Reward: {GetCumulativeReward()}");

        }
        else if (action == 1)
        {
            if (isSleeping)
            {
                Debug.Log("Baby is waking up due to need.");
                isSleeping = false;
                sleepTime = 0;
            }

            animator.SetBool("isCrying", true);
            totalCries++;
            Debug.Log($"Age: {ageGroup}, IsHungry: {hungry}, IsSleepy: {sleepy}, Pooped : {hasPooped}, Cry: 1, Reward: {GetCumulativeReward()}");
            if (hungry || sleepy || hasPooped & !hasCriedThisNeed)
            {
                AddReward(+1f); // appropriate cry
                correctCries++;
                hasCriedThisNeed = true;

                // ENVIRONMENT RESPONSE TO CRY
                if (!userControlResponded)
                {
                    caregiver.RespondToCry(this);
                }


            }
            else
            {
                AddReward(-1f); // cried without needing anything
                falseCries++;
            }
        }

        // Wake baby up after typical sleep time
        //if (isSleeping && GetSleepDurationTarget() > 0 && sleepTime % GetSleepDurationTarget() == 0)
        if (isSleeping && autoWakeEnabled && sleepTime >= GetSleepDurationTarget())
        {
            isSleeping = false;
            sleepTime = 0;
        }

        // End of day
        if (tick >= 1440)
        {
            EvaluateDailyNeeds();
            Academy.Instance.StatsRecorder.Add("Baby/CorrectCries", correctCries);
            Academy.Instance.StatsRecorder.Add("Baby/FalseCries", falseCries);
            Academy.Instance.StatsRecorder.Add("Baby/CryActions", totalCries);
            Academy.Instance.StatsRecorder.Add("Baby/NoAction", totalNoActions);
            EndEpisode();

        }
        Debug.Log($"Tick: {tick}, Age: {ageGroup}, IsHungry: {hungry}, IsSleepy: {sleepy}, IsSleeping{isSleeping}, Pooped : {hasPooped}, Cry: {action}, Reward: {GetCumulativeReward()}");
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        actionsOut.DiscreteActions.Array[0] = (IsHungry() || IsSleepy() || hasPooped) ? 1 : 0;
        //var discreteActionsOut = actionsOut.DiscreteActions;
        //discreteActionsOut[0] = 1; // default do nothing
    }
}
