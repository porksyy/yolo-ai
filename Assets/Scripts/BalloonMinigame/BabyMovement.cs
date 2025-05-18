using UnityEngine;

public class BabyMovement : MonoBehaviour
{
    float movementInput = 0f;
    public float movementSpeed = 5f;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementInput * movementSpeed, rb.linearVelocityY);
    }

    public void MoveLeft()
    {
        movementInput = -1f;
    }

    public void MoveRight()
    {
        movementInput = 1f;
    }

    public void StopMoving()
    {
        movementInput = 0f;
    }
}
