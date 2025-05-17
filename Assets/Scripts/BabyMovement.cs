using UnityEngine;

public class BabyMovement : MonoBehaviour
{

    float horizontalInput;
    float movementSpeed = 7f;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * movementSpeed, rb.linearVelocityY);
    }
}
