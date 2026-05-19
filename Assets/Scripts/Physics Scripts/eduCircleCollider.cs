using UnityEngine;

public class eduCircleCollider : MonoBehaviour
{
    public float radius;
    [SerializeField] float density;
    [SerializeField] eduRigidBody rigidBody;

    void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x, 1);
        radius = transform.localScale.x / 2; //x and y same for circles, scale.x = diameter

        float area = Mathf.PI * Mathf.Pow(radius, 2);
        float mass = area * density;
        if (rigidBody)
        {
            rigidBody.mass = mass;
            rigidBody.inertia = 0.5f * mass * Mathf.Pow(radius, 2); //inertia formula for disc
        }
        else
        {
            Debug.Log("rigidbody not set for circle collider");
        }
    }
}
