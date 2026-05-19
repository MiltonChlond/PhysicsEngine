using UnityEngine;
using static UnityEditor.PlayerSettings;

public class eduBuoyanceField : MonoBehaviour
{
    [SerializeField] float density = 997; //water
    [SerializeField] float surfaceLevel = 0;
    [SerializeField] float maxDepth = 0;
    [SerializeField] Vector2 rangeX = Vector2.zero;

    void Start()
    {
        surfaceLevel = transform.position.y + (transform.localScale.y / 2);
        maxDepth = surfaceLevel - transform.localScale.y;
        rangeX = new Vector2(transform.position.x - (transform.localScale.x / 2), transform.position.x + (transform.localScale.x / 2));
    }

    float CalculateSubmergedArea(float radius, float h)
    {
        h = Mathf.Clamp(h, 0, 2 * radius); //fully submergerd or not at all

        //area of submerged part of circle
        float submergedArea = radius * radius * Mathf.Acos((radius - h) / radius) - (radius - h) * Mathf.Sqrt(2 * radius * h - h * h);
        
        return submergedArea;
    }

    bool CheckIfInside(Vector2 rbPos, float radius)
    {
        if (rbPos.x < rangeX.y && rbPos.x > rangeX.x && rbPos.y < surfaceLevel + (radius / 2) && rbPos.y > maxDepth)//inside
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ApplyBuoyanceForce(eduRigidBody rb, Vector2 gravity)
    {
        eduCircleCollider circleCol = rb.gameObject.GetComponent<eduCircleCollider>();
        if (!circleCol) return;

        if (CheckIfInside((Vector2)rb.transform.position, circleCol.radius))
        {
            Debug.Log("inside");
            float h = surfaceLevel - (rb.transform.position.y - circleCol.radius);
            float submergedArea = CalculateSubmergedArea(circleCol.radius, h);

            Vector2 dragForce = (0.5f * density * submergedArea * rb.velocity * rb.velocity) * -rb.velocity.normalized;

            Vector2 force = -gravity * (density * submergedArea) + dragForce;
            rb.ApplyForce(force);
        }
    }
}
