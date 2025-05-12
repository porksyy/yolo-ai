using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public BabyAgent baby;
    public FloatingHolder floatingHolder;
    
    public void OnFeedButtonPressed()
    {
        if (baby.IsHungry() && !baby.isSleeping)
        {
            FloatingHolder notif = Instantiate(floatingHolder, baby.transform.position, Quaternion.identity) as FloatingHolder;
            notif.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Feeded!";
            baby.Feed();
            Debug.Log("Feeding the baby.");
        }
        else
        {
            if (baby.isSleeping)
            {
                FloatingHolder notif = Instantiate(floatingHolder, baby.transform.position, Quaternion.identity) as FloatingHolder;
                TextMeshPro text = notif.transform.GetChild(0).GetComponent<TextMeshPro>();
                text.text = "Baby is sleeping";
                text.color = Color.red;
            } 
            else
            {
                FloatingHolder notif = Instantiate(floatingHolder, baby.transform.position, Quaternion.identity) as FloatingHolder;
                TextMeshPro text = notif.transform.GetChild(0).GetComponent<TextMeshPro>();
                text.text = "Not hungry";
                text.color = Color.red;
            }
                Debug.Log("The baby is not hungry.");
        }
    }

    public void OnSleepButtonPressed()
    {
        if (baby.IsSleepy())
        {
            baby.PutToSleep();
            FloatingHolder notif = Instantiate(floatingHolder, baby.transform.position, Quaternion.identity) as FloatingHolder;
            notif.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Sleeping!";
        }
        else
        {
            FloatingHolder notif = Instantiate(floatingHolder, baby.transform.position, Quaternion.identity) as FloatingHolder;
            TextMeshPro text = notif.transform.GetChild(0).GetComponent<TextMeshPro>();
            text.text = "Not sleepy";
            text.color = Color.red;
        }
    }

    public void OnCleanButtonPressed()
    {
        if (baby.hasPooped)
        {
            baby.CleanUp();
            FloatingHolder notif = Instantiate(floatingHolder, baby.transform.position, Quaternion.identity) as FloatingHolder;
            notif.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Cleaned!";
            Debug.Log("Cleaning the baby.");
        }
        else
        {
            FloatingHolder notif = Instantiate(floatingHolder, baby.transform.position, Quaternion.identity) as FloatingHolder;
            TextMeshPro text = notif.transform.GetChild(0).GetComponent<TextMeshPro>();
            text.text = "Already clean";
            text.color = Color.red;
        }
    }
}
