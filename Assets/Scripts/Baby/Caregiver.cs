using UnityEngine;

public class Caregiver : MonoBehaviour
{
    public void RespondToCry(BabyAgent baby)
    {
        // Respond to the baby's cry
        if (baby.IsHungry())
        {
            baby.Feed();
        }
        if (baby.IsSleepy())
        {
            baby.PutToSleep();
        }
        if (baby.hasPooped)
        {
            baby.CleanUp();
        }
    }
}
