using UnityEngine;

public class BalloonFall : MonoBehaviour
{
    float wait = 0.3f;
    public GameObject fallingBalloon;

    public void StartSpawning()
    {
        InvokeRepeating("Fall", wait, wait);
    }

    void Fall()
    {
        Instantiate(fallingBalloon, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);
    }

    public void StopSpawning()
    {
        CancelInvoke("Fall");
    }
}
