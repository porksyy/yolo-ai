// BabyAgent.cs
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections.Generic;

public class BabyAgent : Agent
{
    public float hungerLevel = 0f;
    public float hungerIncreaseRate = 0.01f; // Increases faster for younger babies
    public float sleepinessLevel = 0f;
    public float sleepinessIncreaseRate = 0.01f;
    public float timeOfDay = 0f; // Simulated clock time
    public int feedCountToday = 0;
    public int ageInMonths = 0;
    public bool isPoopy = false;

    public BabyEnvironment env;

    // Called at the start of each training episode
    public override void OnEpisodeBegin()
    {
        hungerLevel = 0f;
        sleepinessLevel = 0f;
        feedCountToday = 0;
        timeOfDay = 0f;
        isPoopy = false;

        // Randomize baby's age from 0 to 11 months
        ageInMonths = Random.Range(0, 12);
        AdjustRatesByAge();

        env.ResetFeedingHistory();
        env.ResetSleepingHistory();
    }

    // Set hunger rate based on age (younger babies get hungrier faster)
    void AdjustRatesByAge()
    {
        if (ageInMonths < 3)
        {
            hungerIncreaseRate = 0.03f; // Newborns
            sleepinessIncreaseRate = 0.04f;
        }
        else if (ageInMonths < 6)
        {
            hungerIncreaseRate = 0.02f; // Infants
            sleepinessIncreaseRate = 0.03f;
        }
        else
        {
            hungerIncreaseRate = 0.01f; // Older babies
            sleepinessIncreaseRate = 0.02f;
        }
    }

    // Sends observations to the neural network
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(hungerLevel);             // How hungry the baby is (0 to 1)
        sensor.AddObservation(sleepinessLevel);         // How sleepy the baby is (0 to 1)
        sensor.AddObservation(timeOfDay / 24f);         // Time of day (normalized)
        sensor.AddObservation(feedCountToday);          // How many times fed today
        sensor.AddObservation(ageInMonths / 12f);       // Baby's age (normalized)
        sensor.AddObservation(isPoopy ? 1f : 0f);       // Whether the baby has pooped
    }

    // The agent's decision-making and reward logic
    public override void OnActionReceived(ActionBuffers actions)
    {
        int cry = actions.DiscreteActions[0]; // 0 = stay silent, 1 = cry

        // Hunger increases over time
        hungerLevel += hungerIncreaseRate;
        sleepinessLevel += sleepinessIncreaseRate;

        bool isDiscomfort = hungerLevel >= 0.6f || sleepinessLevel >= 0.6f; 

        // Crying behavior
        if (cry == 1)
        {
            // Crying at correct time (when hungry)
            if (isDiscomfort)
            {
                SetReward(1f); // Positive reward
                if (hungerLevel >= 0.6f)
                {
                    hungerLevel = 0f;
                    feedCountToday++;
                    // Log actual feeding time
                    env.RecordFeedingTime(timeOfDay);

                    // Baby poops if fed enough
                    if (feedCountToday >= 3) isPoopy = true;
                }

                if (sleepinessLevel >= 0.6f)
                {
                    sleepinessLevel = 0f;
                    // Log actual sleeping time
                    env.RecordSleepTime(timeOfDay);
                }
            }
            else
            {
                SetReward(-0.5f); // Negative reward for false cry
            }
        }
        else
        {
            // Penalty if baby is very hungry but doesn’t cry
            if (hungerLevel >= 0.8f || sleepinessLevel >= 0.8f)
            {
                SetReward(-1f);
            }
        }

        // Bonus for pooping (could be used for diaper logic later)
        if (isPoopy)
        {
            SetReward(0.2f);
            isPoopy = false;
        }

        // Simulate time passing
        timeOfDay += Time.deltaTime / 10f; // Slower time progression

        // End the day
        if (timeOfDay >= 24f)
        {
            EndEpisode();
        }
    }

    // Optional manual control for testing
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        actionsOut.DiscreteActions.Array[0] = (hungerLevel >= 0.7f || sleepinessLevel >= 0.7f) ? 1 : 0;
    }
}
