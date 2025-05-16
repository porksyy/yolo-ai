using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;

    private FoodManager foodManager;

    public bool isGoodFood = false; // Assigned by FoodManager

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        foodManager = FindObjectOfType<FoodManager>(); // Add this line
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        bool droppedOnBaby = (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Baby"));

        // Always reset the position first
        rectTransform.anchoredPosition = startPosition;

        if (droppedOnBaby)
        {
            HealthManager healthManager = FindObjectOfType<HealthManager>();
            if (healthManager != null)
            {
                if (isGoodFood)
                {
                    healthManager.AddHealth();
                }
                else
                {
                    healthManager.SubtractHealth();
                }
            }

            // Just randomize new foods; don't clear sprite manually
            if (foodManager != null)
            {
                foodManager.RandomizeFoods();
            }
        }
    }
}
