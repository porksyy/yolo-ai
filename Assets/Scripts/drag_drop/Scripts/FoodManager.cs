using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Don't forget this for UI Images

// --- You add the FoodSet class HERE ---
[System.Serializable]
public class FoodSet
{
    public FoodInfo food1;
    public FoodInfo food2;
    public FoodInfo food3;
}

// --- Then your manager script ---
public class FoodManager : MonoBehaviour
{
    public List<FoodSet> foodSets;

    public Image container1;
    public Image container2;
    public Image container3;

    void Start()
    {
        RandomizeFoods();
    }


    public void RandomizeFoods()
    {
        int randomIndex = Random.Range(0, foodSets.Count);
        FoodSet selectedSet = foodSets[randomIndex];

        SetFood(container1, selectedSet.food1);
        SetFood(container2, selectedSet.food2);
        SetFood(container3, selectedSet.food3);
    }

    void SetFood(Image container, FoodInfo foodInfo)
    {
        container.sprite = foodInfo.foodSprite;

        DragFood dragFood = container.GetComponent<DragFood>();
        if (dragFood == null)
        {
            dragFood = container.gameObject.AddComponent<DragFood>();
        }

        dragFood.enabled = true; // <<< make sure it's enabled
        dragFood.isGoodFood = foodInfo.isGoodFood;
    }

}
