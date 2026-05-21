using NUnit.Framework;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class eduForces : MonoBehaviour
{
    [SerializeField] eduRigidBody trajectoryCircle; //exercise 1.3
    Vector2 startPosTrajectory;
    Vector2 startVelTrajectory;

    //forces
    [SerializeField] bool gravityEnabled;
    [SerializeField] Vector2 gravity;

    [SerializeField] bool torqueEnabled;
    [SerializeField] float torque;

    void Start()
    {
        gravity = new Vector2(0, -9.82f);
    }

    void DrawLineTrajectory() //analytical/accurate trajectory for exercise 1.3
    {
        int amountOfLines = 100;
        float totalTime = 3; //controls the length of trajectory

        Vector2 x0 = startPosTrajectory;
        Vector2 v0 = startVelTrajectory;

        float dt = totalTime / amountOfLines;

        Vector2 previous = x0;

        for(int i = 1;  i < amountOfLines; i++)
        {
            float t = i * dt; //time passed since start
            Vector2 current = x0 + v0 * t + 0.5f * gravity * Mathf.Pow(t, 2);

            Debug.DrawLine(new Vector3(previous.x, previous.y, 0), new Vector3(current.x, current.y, 0), Color.white);

            previous = current;
        }
    }

    void FixedUpdate()
    {
        eduRigidBody[] RBs = FindObjectsByType<eduRigidBody>(FindObjectsSortMode.None);
        eduWindField[] windFields = FindObjectsByType<eduWindField>(FindObjectsSortMode.None);
        eduBuoyanceField[] buoyanceFields = FindObjectsByType<eduBuoyanceField>(FindObjectsSortMode.None);
        eduExplosionPoint[] explosionPoints = FindObjectsByType<eduExplosionPoint>(FindObjectsSortMode.None);
        foreach(eduRigidBody rb in RBs)
        {
            if(gravityEnabled)
                rb.ApplyForce(gravity * rb.mass);
            
            if(torqueEnabled)
                rb.ApplyTorque(torque);

            //wind
            foreach(eduWindField windField in windFields)
            {
                windField.ApplyWindForce(rb);
            }

            //buoyance
            foreach(eduBuoyanceField buoyanceField in buoyanceFields)
            {
                buoyanceField.ApplyBuoyanceForce(rb, gravity);
            }
        }

        //explosion
        foreach (eduExplosionPoint explosion in explosionPoints)
        {
            explosion.UpdateExplosions(RBs);
        }
    }

    public void ApplyExplosionForce(eduRigidBody rb, eduExplosion explosion)
    {
        float distance = Vector2.Distance(explosion.pos, rb.transform.position);
        if (distance < explosion.explosionRadius) //within explosion
        {
            Vector2 direction = ((Vector2)rb.transform.position - (Vector2)explosion.pos).normalized;

            float powerFalloff = 1 - (distance / explosion.explosionRadius);

            Vector2 impulse = direction * explosion.explosionPower * powerFalloff / rb.mass;
            rb.ApplyImpulse(impulse);
            //StartCoroutine(MakeExplosionVisible());
        }
    }

    void Update()
    {
        //draw trajectory for exercise 1.3
        if (trajectoryCircle != null)
        {
            startPosTrajectory = trajectoryCircle.transform.position;
            startVelTrajectory = trajectoryCircle.velocity;
            DrawLineTrajectory();
        }
    }
}
