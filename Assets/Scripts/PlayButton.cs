using UnityEngine;
using UnityEngine.UI;

public class GameStartController : MonoBehaviour
{
    public GameObject playButton;      
    public BalloonFall balloonFall;    
    public Timer gameTimer;            
    public GameObject gameUI;         

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
