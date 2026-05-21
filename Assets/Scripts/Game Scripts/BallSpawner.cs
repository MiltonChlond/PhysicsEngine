using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public bool isAlive = true;

    void Start()
    {
        
    }

    public void Hit()
    {
        isAlive = false;
    }
}
