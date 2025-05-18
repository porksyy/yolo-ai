using UnityEngine;

public class FallDestroyer : MonoBehaviour
{
    private Timer gameTimer;

    private void Start()
    {
        gameTimer = FindFirstObjectByType<Timer>();

        if (gameTimer == null)
        {
            Debug.LogWarning("GameTimer not found in scene!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Baby"))
        {
            Debug.Log("Balloon hit the baby!");

            if (gameTimer == null)
            {
                gameTimer = FindFirstObjectByType<Timer>(); // fallback, just in case
            }

            if (gameTimer != null)
            {
                gameTimer.StopTimerEarly();
            }
            else
            {
                Debug.LogError("GameTimer reference is still null!");
            }

            Destroy(this.gameObject);
        }
    }
}
