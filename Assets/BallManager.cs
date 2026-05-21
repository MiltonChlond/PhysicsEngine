using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class BallManager : MonoBehaviour
{
    [SerializeField] eduForces eForces;
    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject ball;
    [SerializeField] GameObject ballPrefab;

    List<GameObject> balls;

    void Start()
    {
        balls = new List<GameObject>();
    }

    public void AddBall()
    {
        float screenSize = Camera.main.orthographicSize * Camera.main.aspect;
        float minX = -screenSize;
        float maxX = screenSize;

        float spawnPosX = Random.Range(minX - 2, maxX - 2);
        Vector3 spawnPos = new Vector3(spawnPosX, 4, 0);

        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        balls.Add(ball);
    }

    public void Explosion(eduExplosion exp)
    {
        if(Vector2.Distance(ball.transform.position, exp.pos) < exp.explosionRadius)
            eForces.ApplyExplosionForce(ball.GetComponent<eduRigidBody>(), exp);

        foreach(GameObject ball in balls)
        {
            if (Vector2.Distance(ball.transform.position, exp.pos) < exp.explosionRadius)
                eForces.ApplyExplosionForce(ball.GetComponent<eduRigidBody>(), exp);
        }
    }

    void Update()
    {
        for (int i = balls.Count - 1; i >= 0; i--)
        {
            if (balls[i].transform.position.y < -2)
            {
                Destroy(balls[i].gameObject);
                balls.RemoveAt(i);
            }
        }

        if(ball.transform.position.y < -2)
        {
            gameManager.GameOver();
        }
    }
}
