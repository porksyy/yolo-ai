using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 30f;
    public bool timerIsRunning = true;
    public TextMeshProUGUI timerText;
    public GameObject gameOverScreen;

    void Update()
    {
        Debug.Log("Time remaining: " + timeRemaining);

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                ShowGameOver();
            }
        }
    }

    public void StopTimerEarly()
    {
        timerIsRunning = false;
        ShowGameOver();
    }

    void UpdateTimerUI()
    {
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }

    void ShowGameOver()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        // Stop balloon spawning
        BalloonFall balloonFall = FindAnyObjectByType<BalloonFall>();
        if (balloonFall != null)
            balloonFall.StopSpawning();

        // Stop existing balloons
        GameObject[] balloons = GameObject.FindGameObjectsWithTag("Balloon");
        foreach (GameObject b in balloons)
        {
            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Unity 6
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic; // freeze in place
            }
        }

        GameObject playButton = GameObject.Find("Btn_Play");
        if (playButton != null)
            playButton.SetActive(false);

    }
}
