using UnityEngine;

public class FloatingHolder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 1f);
        transform.position -= new Vector3(0, 1f, 0);
    }
}
