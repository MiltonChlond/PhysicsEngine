using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField] GameObject brickPrefab;
    List<Brick> bricks;

    [SerializeField] List<GameObject> spawnPoints;

    float spawnTime = 5;

    Coroutine coroutine;

    void Start()
    {
        bricks = new List<Brick>();
        coroutine = StartCoroutine(SpawnBricks());
    }

    IEnumerator SpawnBricks()
    {
        MoveBricks();
        foreach(GameObject sp in spawnPoints)
        {
            if(SpawnBrick())
            {
                GameObject brick = Instantiate(brickPrefab, sp.transform.position, Quaternion.identity);
                bricks.Add(brick.GetComponent<Brick>());
            }
        }

        yield return new WaitForSeconds(spawnTime);
        coroutine = StartCoroutine(SpawnBricks());
    }

    bool SpawnBrick()
    {
        if (Random.Range(0.0f, 1.0f) < 0.7f)
            return true;
        else
            return false;
    }

    void MoveBricks()
    {
        foreach(Brick brick in bricks)
        {
            float distance = 2.5f;
            Vector3 currPos = brick.transform.position;
            brick.transform.position = new Vector3(currPos.x, currPos.y - distance, currPos.z);
        }
    }

    public void OnGameOver()
    {
        StopCoroutine(coroutine);
    }
}
