using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gameBoardPanel;

    public void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked");
        startPanel.SetActive(false);
        gameBoardPanel.SetActive(true);
    }

}
