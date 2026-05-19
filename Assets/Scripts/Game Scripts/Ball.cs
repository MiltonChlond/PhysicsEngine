using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] eduRigidBody rb;

    float initialSpeed = 5;
    Vector3 initialDirection = Vector3.up;

    void Start()
    {
        DecideInitialVelocity();
    }

    void DecideInitialVelocity()
    {
        float x = Random.Range(-1.0f, 1.0f);
        initialDirection = new Vector3(x, 1, 0);
        rb.velocity = initialDirection * initialSpeed;
    }
}
