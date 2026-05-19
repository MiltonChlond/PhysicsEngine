using UnityEngine;

public class eduPlaneCollider : MonoBehaviour
{
    public Vector2 pos;
    public Vector2 normal;
    float size = 0;
    [SerializeField] eduRigidBody rigidBody;
    [SerializeField] SpriteRenderer sprite;    

    void Start()
    {
        size = transform.localScale.x;
        rigidBody.mass = Mathf.Infinity;
        normal = transform.up;
        pos = (Vector2)transform.position + (normal * (transform.localScale.y / 2));
    }

    void Update()
    {
        Debug.DrawLine(pos, pos + normal, Color.white); //showcase normal
        Debug.DrawLine(pos, pos + (Vector2)transform.right * size/2, Color.purple);
        Debug.DrawLine(pos, pos + (Vector2)(-transform.right * size/2), Color.purple);
    }
}
