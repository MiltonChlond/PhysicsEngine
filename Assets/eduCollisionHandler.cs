using UnityEngine;

public class eduCollisionHandler : MonoBehaviour
{
    [SerializeField] BallManager ballManager;
    public void HandleCollision(eduRigidBody body1, eduRigidBody body2)
    {
        if(body1.transform.CompareTag("Ball") && body2.transform.CompareTag("Brick"))
        {
            Brick brick;
            if(brick =  body2.GetComponent<Brick>())
            {
                brick.BrickHit();
            }
        }

        if(body2.transform.CompareTag("BallSpawner") && body1.transform.CompareTag("Ball"))
        {
            ballManager.AddBall();
            body2.GetComponent<BallSpawner>().Hit();
        }
    }
}
