using UnityEngine;
using UnityEngine.UI;

public class ResponsiveGrid : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public int columns = 4;
    public int rows = 4;

    void Start()
    {
        RectTransform rt = gridLayout.GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;

        float cellWidth = (width - gridLayout.padding.left - gridLayout.padding.right - (gridLayout.spacing.x * (columns - 1))) / columns;
        float cellHeight = (height - gridLayout.padding.top - gridLayout.padding.bottom - (gridLayout.spacing.y * (rows - 1))) / rows;

        float cellSize = Mathf.Min(cellWidth, cellHeight); // Ensures square cells that fit

        gridLayout.cellSize = new Vector2(cellSize, cellSize);
    }
}
