using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int health = 0;
    public int dragDropCount = 0; // << New

    public GameObject gameOverPanel; // << New (assign in Inspector)
    public Text scoreText;           // << New (assign in Inspector)

    public Image babyMood;

    public Sprite happyMood;
    public Sprite cryingMood;

    public void AddHealth()
    {
        health += 10;
        Debug.Log("Good Food! Health: " + health);
        ChangeMood(true);
        IncrementDragDrop();
    }

    public void SubtractHealth()
    {
        health -= 10;
        Debug.Log("Bad Food! Health: " + health);
        ChangeMood(false);
        IncrementDragDrop();
    }

    public void ChangeMood(bool goodFood)
    {
        if (goodFood && happyMood != null)
        {
            babyMood.sprite = happyMood;
        }
        else if (!goodFood && cryingMood != null)
        {
            babyMood.sprite = cryingMood;
        }
        else
        {
            Debug.Log("Cannot");
        }
    }

    void IncrementDragDrop()
    {
        dragDropCount++;

        if (dragDropCount >= 10)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("Game Over! Final Health: " + health);

        if (gameOverPanel != null && scoreText != null)
        {
            gameOverPanel.SetActive(true);
            scoreText.text = "Final Score: " + health;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
