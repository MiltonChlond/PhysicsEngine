using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    [SerializeField] float paddleHalfWidth = 0.5f;

    void Update()
    {
        Vector2 mousePos =
            Mouse.current.position.ReadValue();

        Vector3 worldPos =
            Camera.main.ScreenToWorldPoint(
                new Vector3(mousePos.x, mousePos.y, 10f)
            );

        float screenHalfWidth =
            Camera.main.orthographicSize *
            Camera.main.aspect;

        float camX =
            Camera.main.transform.position.x;

        float minX =
            camX - screenHalfWidth + paddleHalfWidth;

        float maxX =
            camX + screenHalfWidth - paddleHalfWidth;

        float clampedX =
            Mathf.Clamp(worldPos.x, minX, maxX);

        transform.position = new Vector3(
            clampedX,
            transform.position.y,
            0f
        );
    }
}