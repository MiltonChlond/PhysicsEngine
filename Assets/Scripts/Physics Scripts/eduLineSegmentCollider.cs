using UnityEngine;

public class eduLineSegmentCollider : MonoBehaviour
{
    [SerializeField] eduRigidBody rb;
    [SerializeField] SpriteRenderer sprite;

    [SerializeField] public float size;
    [SerializeField] public Vector2 posMid;

    public Vector2 pos1;
    public Vector2 pos2;

    [SerializeField] public Vector2 normal;

    [SerializeField] bool overrideColliderSpawnPoint = false; //if true, manually set size and pos

    void Start()
    {
        if (!overrideColliderSpawnPoint)
        {
            size = transform.localScale.x;
            rb.mass = Mathf.Infinity;
            normal = transform.up;
        }
            posMid = (Vector2)transform.position + (normal * (transform.localScale.y / 2));
            pos1 = posMid + (Vector2)transform.right * size / 2;
            pos2 = posMid + -(Vector2)transform.right * size / 2;
    }

    void Update()
    {
        posMid = (Vector2)transform.position + (normal * (transform.localScale.y / 2));
        pos1 = posMid + (Vector2)transform.right * size / 2;
        pos2 = posMid + -(Vector2)transform.right * size / 2;

        Debug.DrawLine(posMid, posMid + normal, Color.white);
        Debug.DrawLine(posMid, pos1, Color.red);
        Debug.DrawLine(posMid, pos2, Color.red);
    }
}
