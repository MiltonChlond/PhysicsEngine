using UnityEngine;

public class PaddleController : MonoBehaviour
{
    PaddleInput input;
    [SerializeField] float moveSpeed = 1f;

    void Start()
    {
        input = new PaddleInput();
        input.Enable();
    }

    void Update()
    {
        Vector3 moveVector = Vector3.zero;
        if(input.Paddle.Right.IsPressed())
        {
            moveVector.x += 1;
        }
        if(input.Paddle.Left.IsPressed())
        {
            moveVector.x -= 1;
        }
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}
