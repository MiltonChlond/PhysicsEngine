using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] BallManager ballManager;
    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject brickPrefab;
    [SerializeField] GameObject ballPrefab;
    List<Brick> bricks;
    List<GameObject> balls;

    [SerializeField] List<GameObject> spawnPoints;

    float moveSpeed = 0.5f;
    float totalDistance = 0f;
    const float spacingY = 2.5f;
    bool spawnedFirstRow = false;

    [SerializeField] GameObject explosionVisual;
    const float expRadius = 14;
    const float expPower = 1000;

    void Start()
    {
        bricks = new List<Brick>();
        balls = new List<GameObject>();
    }

    public void SpawnBricks()
    {
        if (totalDistance <= spacingY && spawnedFirstRow)
            return;

        totalDistance = 0f;
        spawnedFirstRow = true;
        foreach(GameObject sp in spawnPoints)
        {
            if(SpawnBall())
            {
                GameObject ball = Instantiate(ballPrefab, sp.transform.position, Quaternion.identity);
                balls.Add(ball);
                continue;
            }
            else if(SpawnBrick())
            {
                GameObject brick = Instantiate(brickPrefab, sp.transform.position, Quaternion.identity);
                bricks.Add(brick.GetComponent<Brick>());
            }
        }
        moveSpeed += 0.005f;
    }

    bool SpawnBall()
    {
        if (Random.Range(0.0f, 1.0f) < 0.08f)
            return true;
        else
            return false;
    }

    bool SpawnBrick()
    {
        if (Random.Range(0.0f, 1.0f) < 0.55f)
            return true;
        else
            return false;
    }

    void MoveBricks()
    {
        float deltaY = moveSpeed * Time.deltaTime;
        totalDistance += deltaY;
        foreach (Brick brick in bricks)
        {
            Vector3 currPos = brick.transform.position;
            brick.transform.position = new Vector3(currPos.x, currPos.y - deltaY, currPos.z);
        }
        foreach(GameObject ball in balls)
        {
            Vector3 currPos = ball.transform.position;
            ball.transform.position = new Vector3(currPos.x, currPos.y - deltaY, currPos.z);
        }
    }

    void CreateExplosion(Vector3 pos)
    {
        eduExplosion exp = new eduExplosion(pos, expRadius, expPower);
        Instantiate(explosionVisual, pos, Quaternion.identity);

        ballManager.Explosion(exp);

        foreach(GameObject ball in balls)
        {
            ballManager.AddBall();
            ball.GetComponent<BallSpawner>().Hit();
        }
        foreach(Brick b in bricks)
        {
            if (Vector2.Distance(b.transform.position, exp.pos) < 3)
            {
                b.BrickHit();
            }
        }
    }

    private void Update()
    {
        List<Vector3> explosions = new List<Vector3>();
        for(int i = bricks.Count - 1; i >= 0; i--)
        {
            bool destroy = false;
            if (bricks[i].transform.position.y < -3)
            {
                gameManager.BrickHitBottom();
                destroy = true;
            }
            if (!bricks[i].isAlive)
            {
                if (bricks[i].type == Brick.BrickType.Explosive)
                {
                    explosions.Add(bricks[i].transform.position);
                }
                destroy = true;
            }
             
            if(destroy)
            {
                Destroy(bricks[i].gameObject);
                bricks.RemoveAt(i);
            }
        }

        foreach(Vector3 pos in explosions)
        {
            CreateExplosion(pos);
        }

        for (int i = balls.Count - 1; i >= 0; i--)
        {
            bool destroy = false;
            BallSpawner ball = balls[i].GetComponent<BallSpawner>();
            if (!ball.isAlive)
            {
                destroy = true;
            }
            if(ball.transform.position.y < -3)
            {
                destroy = true;
            }

            if(destroy)
            {
                Destroy(balls[i].gameObject);
                balls.RemoveAt(i);
            }
        }
        MoveBricks();
        SpawnBricks();
    }
}
