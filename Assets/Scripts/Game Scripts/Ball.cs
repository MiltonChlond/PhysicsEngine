using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] eduRigidBody rb;

    float initialSpeed = 12;
    Vector3 initialDirection = Vector3.up;
    float speedCorrectionFactor = 100;

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

    private void FixedUpdate()
    {
        if(rb.velocity.magnitude > initialSpeed)
        {
            rb.ApplyForce(-rb.velocity.normalized * speedCorrectionFactor);
        }
        if(rb.velocity.magnitude < initialSpeed)
        {
            rb.ApplyForce(rb.velocity.normalized * speedCorrectionFactor);
        }
    }
}
