using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null) return;

        transform.localScale = Vector3.one;

        float width = sr.bounds.size.x;
        float height = sr.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 scale = transform.localScale;
        scale.x = worldScreenWidth / width;
        scale.y = worldScreenHeight / height;
        transform.localScale = scale;
    }
}
