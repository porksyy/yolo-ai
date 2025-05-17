using UnityEngine;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public GameObject playButton;      // Assign your Play Button GameObject here
    public BalloonFall balloonFall;    // Assign BalloonFall script here
    public Timer gameTimer;            // Assign Timer script here
    public GameObject gameUI;          // Any gameplay UI to enable on start

    void Start()
    {
        // Make sure gameplay UI is off until play pressed
        if (gameUI != null) gameUI.SetActive(false);
        if (balloonFall != null) balloonFall.StopSpawning();
        if (gameTimer != null) gameTimer.timerIsRunning = false;
    }

    public void OnPlayButtonPressed()
    {
        if (playButton != null)
            playButton.SetActive(false);

        if (gameUI != null)
            gameUI.SetActive(true);

        if (balloonFall != null)
            balloonFall.StartSpawning();

        if (gameTimer != null)
            gameTimer.timerIsRunning = true;
    }
}
