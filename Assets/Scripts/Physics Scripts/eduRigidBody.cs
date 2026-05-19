using NUnit.Framework;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class eduRigidBody : MonoBehaviour
{
    [SerializeField] public Vector2 velocity = Vector3.zero;
    [SerializeField] public float angularVelocity;

    [SerializeField] Vector2 F;
    [SerializeField] float T;

    [SerializeField] public float mass;
    [SerializeField] public float inertia;
    [SerializeField] public float cor;

    [SerializeField] int skipFrames;
    int frame;

    //exercise 1.4 and 1.5
    //bool angularVelReachedzero = false;

    void Start()
    {
        angularVelocity = 0;
        frame = 0;
        //cor
        //skipframes
    }

    public void ApplyForce(Vector2 f)
    {
        F += f;
    }

    public void ApplyTorque(float t)
    {
        T += t;
    }

    public void ApplyImpulse(Vector2 i)
    {
        velocity += i;
    }

    void FixedUpdate()
    {
        //frame skip
        frame++;
        if (skipFrames != 0)//divide by zero check, if zero we dont skip
        {
            if (!(frame % skipFrames == 0))
            {
                F = Vector2.zero;
                T = 0;
                return;
            }
        }
        
        float dt = Time.deltaTime * (skipFrames + 1);

        Vector2 acceleration = Vector2.zero;

        //apply forces to acceleration
        if(mass != 0 && mass != Mathf.Infinity)
            acceleration += F / mass;

        //apply acceleration to velocity
        velocity += acceleration * dt;

        transform.position += new Vector3(velocity.x, velocity.y, 0) * dt;

        if(inertia != 0 && inertia != Mathf.Infinity)
            angularVelocity += (T / inertia) * dt;

        transform.Rotate(0, 0, angularVelocity * Mathf.Rad2Deg * dt);

        //exercise 1.4 and 1.5
        //if (angularVelocity <= 0 && !angularVelReachedzero)
        //{
        //    Debug.Log("Circle name: " + transform.name + "time: " + frame * 0.02f); //maybe not accurate with 0.02f but gave consistent results
        //    angularVelReachedzero = true;
        //}
        //result for exercise 1.4
        //  simulation = 1.32 seconds
        //  theoretical solution = 1.31 seconds
        //result for exercise 1.5
        //  from simulation:
        //      circle 1: 1.32 sec
        //      circle 2: 2.62 sec
        //      circle 3: 3.94 sec
        //      circle 4: 5.24 sec
        //  theoretical:
        //      circle 1: 1.31 sec
        //      circle 2: 2.62 sec
        //      circle 3: 3.93 sec
        //      circle 4: 5.24 sec
        //difference in results from simulation and theoretical calculations can be caused by frame * 0.02,
        //meaning that the time printed from the simulation can only be from a multiple of 0.02 and cant be the same as the theoretical solutions resulting in an uneven number
        //results exercise 1.6
        //  circle 1: 1.32   //skip frame = 0
        //  circle 2: 1.02   //skip frame = 3
        //  circle 3: 1.2   //skip frame = 6


        F = Vector2.zero;
        T = 0;
    }
}
