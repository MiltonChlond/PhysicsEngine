using UnityEngine;

public class eduLineSegmentCollider : MonoBehaviour
{
    [SerializeField] eduRigidBody rb;
    [SerializeField] SpriteRenderer sprite;

    public float size;
    public Vector2 posMid;

    public Vector2 pos1;
    public Vector2 pos2;

    public Vector2 normal;

    void Start()
    {
        size = transform.localScale.x;
        rb.mass = Mathf.Infinity;
        normal = transform.up;
        posMid = (Vector2)transform.position + (normal * transform.localScale.y);
        pos1 = posMid + (Vector2)transform.right * size/2;
        pos2 = posMid + -(Vector2)transform.right * size/2;
    }

    void Update()
    {
        Debug.DrawLine(posMid, posMid + normal, Color.white);
        Debug.DrawLine(posMid, pos1, Color.red);
        Debug.DrawLine(posMid, pos2, Color.red);
    }
}
