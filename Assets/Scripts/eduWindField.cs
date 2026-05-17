using UnityEngine;

public class eduWindField : MonoBehaviour
{
    //this class will be used to create wind zones in the scene,
    //the eduForces class will find all wind zones and call ApplyWindForce() for all rigid bodies

    [SerializeField] public Vector2 posMid = Vector2.zero;
    [SerializeField] public Vector2 size = Vector2.zero;
    [SerializeField] public Vector2 windDirection = Vector2.zero;
    [SerializeField] public float windPower = 0.0f;
    [SerializeField] public float airDensity = 1.225f; //air at sea level
    [SerializeField] public float dragCoefficient = 0.0f;

    LineRenderer lineRenderer; //to show wind direction
    
    void Start()
    {
        size = transform.localScale;
        posMid = transform.position;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.sortingOrder = 10;
        lineRenderer.useWorldSpace = true;
    }

    (bool, float) CheckIfInside(Vector2 rbPos) //returns true if rb in field, and what percentage of wind power will be applied
    {
        Vector2 rangeX = new Vector2(posMid.x - size.x/2, posMid.x + size.x/2);
        Vector2 rangeY = new Vector2(posMid.y - size.y/2, posMid.y + size.y/2);
        if(rbPos.x > rangeX.x && rbPos.x < rangeX.y && rbPos.y > rangeY.x && rbPos.y < rangeY.y) //inside windField
        {
            Vector2 localPos = rbPos - posMid;
            float projectedToWindDir = Vector2.Dot(localPos, windDirection.normalized);
            float maxProjected = Mathf.Abs(size.x * windDirection.normalized.x) / 2 +
                                 Mathf.Abs(size.y * windDirection.normalized.y) / 2;
            float windPowerPercent = (projectedToWindDir + maxProjected) / (2 * maxProjected); //range 0 to 1


            return (true, windPowerPercent);
        }
        else
        {
            return (false, 0f);
        }
    }

    public void ApplyWindForce(eduRigidBody RB)
    {
        var result = CheckIfInside((Vector2)RB.transform.position);
        if(result.Item1)
        {
            float projectedArea = 0;
            eduCircleCollider circleCol = RB.gameObject.GetComponent<eduCircleCollider>();
            if (!circleCol) return;

            projectedArea = circleCol.radius * 2;
            
            Vector2 force = 0.5f * dragCoefficient * airDensity * windDirection.normalized * windPower * projectedArea * (1 - result.Item2);
            RB.ApplyForce(force);
        }
    }

    void Update()
    {
        Vector2 start = posMid;
        Vector2 end = posMid + windDirection.normalized * 1;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
