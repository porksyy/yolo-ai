using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    [SerializeField] Card cardPrefab;
    [SerializeField] Transform gridTransform;
    [SerializeField] Sprite[] sprites;
    //[SerializeField] private float delay = 0.3f;
    [SerializeField] private GameObject completed;
    [SerializeField] private GameObject timeCount;
    [SerializeField] private GameObject currentScore;
    [SerializeField] private GameObject highScore;


    private float elapsedTime = 0;
    private bool isGameActive = true;


    private List<Sprite> spritePairs;

    Card firstSelected;
    Card secondSelected;

    int matchCounts;

    private void Start()
    {
        PrepareSprites();
        CreateCards();
        Debug.Log($"complted: {completed}");
        if (completed != null)
            completed.SetActive(false);

        // Show high score at start
        int prevHighScore = PlayerPrefs.GetInt("HighScore", int.MaxValue);
        Debug.Log($"hs: {prevHighScore}");
        if (prevHighScore != int.MaxValue)
            highScore.GetComponent<TextMeshProUGUI>().text = prevHighScore + "s";
        else
            highScore.GetComponent<TextMeshProUGUI>().text = "-";
    }

    private void Update()
    {
        if (isGameActive)
        {
            elapsedTime += Time.deltaTime;
            timeCount.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(elapsedTime) + "s";
        }
    }

    void CreateCards()
    {
        for(int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.controller = this;
        }
    }

    public void SetSelected(Card card)
    {
        if (!card.isSelected)
        {
            card.Show();

            if (firstSelected == null)
            {
                firstSelected = card;
                return;
            }
            
            if (secondSelected == null)
            {
                secondSelected = card;
                StartCoroutine(CheckMatching(firstSelected, secondSelected));
                firstSelected = null;
                secondSelected = null;
            }
        }
    }

    IEnumerator CheckMatching(Card a, Card b)
    {
        yield return new WaitForSeconds(0.3f);
        if (a.iconSprite == b.iconSprite)
        {
            matchCounts++;
            if (matchCounts >= spritePairs.Count / 2)
            {
                isGameActive = false;
                int currentTime = Mathf.RoundToInt(elapsedTime);
                if (completed != null)
                {
                    completed.SetActive(true);
                    currentScore.GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(elapsedTime) + "s";

                    // TODO - load highScore
                    int prevHighScore = PlayerPrefs.GetInt("HighScore", int.MaxValue);


                    if (currentTime < prevHighScore)
                    {
                        // TODO - save highScore
                        PlayerPrefs.SetInt("HighScore", currentTime);
                        PlayerPrefs.Save();
                        prevHighScore = currentTime;
                    }

                    if (prevHighScore != int.MaxValue)
                        highScore.GetComponent<TextMeshProUGUI>().text = prevHighScore + "s";
                    else
                        highScore.GetComponent<TextMeshProUGUI>().text = "-";
                }
                    
                    
                PrimeTween.Sequence.Create()
                    .Chain(PrimeTween.Tween.Scale(gridTransform, Vector3.one * 1.2f, 0.2f, ease: PrimeTween.Ease.OutBack))
                    .Chain(PrimeTween.Tween.Scale(gridTransform, Vector3.one, 0.1f));
                
            
            }
        }
        else
        {
            a.Hide();
            b.Hide();
        }
    }
    private void PrepareSprites()
    {
        spritePairs = new List<Sprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            // adding sprite 2 times to make it pair
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }

        ShuffleSprites(spritePairs);
    }

    void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }
}


