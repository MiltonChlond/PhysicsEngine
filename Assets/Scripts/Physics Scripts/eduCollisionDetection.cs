using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class eduCollisionDetection : MonoBehaviour
{
    eduCircleCollider[] circleCol;
    eduLineSegmentCollider[] lineSegCol;
    eduPlaneCollider[] planeCol;

    void Start()
    {
        circleCol = FindObjectsByType<eduCircleCollider>(FindObjectsSortMode.None);
        lineSegCol = FindObjectsByType<eduLineSegmentCollider>(FindObjectsSortMode.None);
        planeCol = FindObjectsByType<eduPlaneCollider>(FindObjectsSortMode.None);
    }

    void FixedUpdate()
    {
        CircleCircleCheck();
        CirclePlaneCheck();
        CircleLineCheck();
    }

    void CirclePlaneCheck()
    {
        if(circleCol != null && planeCol != null)
        {
            if(circleCol.Length > 0 && planeCol.Length > 0)
            {
                foreach(eduCircleCollider circle in circleCol)
                {
                    foreach(eduPlaneCollider plane in planeCol)
                    {
                        Vector2 diff = (Vector2)circle.transform.position - plane.pos;
                        float distance = Vector2.Dot(diff, plane.normal.normalized);
                        float penetration = circle.radius - distance;
                        if (penetration > 0)
                        {
                            eduRigidBody circleRB = circle.gameObject.GetComponent<eduRigidBody>();
                            eduRigidBody planeRB = plane.gameObject.GetComponent<eduRigidBody>();

                            OverlapCorrection(circleRB, planeRB, penetration, plane.normal.normalized);

                            //check if circle is moving away from plane
                            if (Vector2.Dot(circleRB.velocity, plane.normal.normalized) < 0)
                            {
                                CirclePlaneCollision(circleRB, plane.normal.normalized);
                            }
                        }
                    }
                }
            }
        }
    }

    void CircleCircleCheck()
    {
        if (circleCol != null)
        {
            for(int i = 0; i < circleCol.Length; i++)
            {
                eduCircleCollider circle1 = circleCol[i];
                eduRigidBody circle1RB = circle1.gameObject.GetComponent<eduRigidBody>();

                for (int j = i + 1; j < circleCol.Length; j++)
                {
                    eduCircleCollider circle2 = circleCol[j];
                    eduRigidBody circle2RB = circle2.gameObject.GetComponent<eduRigidBody>();

                    float distance = Vector2.Distance((Vector2)circle1.transform.position, (Vector2)circle2.transform.position);
                    float penetration = circle1.radius + circle2.radius - distance;
                    if (penetration > 0)
                    {
                        Vector2 n = (circle2.transform.position - circle1.transform.position).normalized;
                        OverlapCorrection(circle1RB, circle2RB, penetration, n);

                        Vector2 Vrel = circle2RB.velocity - circle1RB.velocity;
                        if (Vector2.Dot(Vrel, n.normalized) < 0) //if not moving apart apply impulse
                        {
                            CircleCircleCollision(circle1RB, circle2RB, Vrel, n);
                        }
                    }
                }
            }
        }
    }

    void CircleLineCheck()
    {
        if (circleCol != null && lineSegCol != null)
        {
            foreach (eduCircleCollider circle in circleCol)
            {
                eduRigidBody circleRB = circle.gameObject.GetComponent<eduRigidBody>();
                foreach (eduLineSegmentCollider line in lineSegCol)
                {
                    eduRigidBody lineRB = line.gameObject.GetComponent<eduRigidBody>();

                    Vector2 diff = (Vector2)circle.transform.position - line.posMid;
                    Vector2 n = line.normal.normalized;
                    float distance = Vector2.Dot(diff, n);

                    if (distance < 0) //no collision / position correction if circles comes from the "not solid side of the line"
                        continue;

                    //find closest end point of line to the circle
                    Vector2 closestEndPoint;
                    if(Vector2.Distance((Vector2)circle.transform.position, line.pos1) > Vector2.Distance((Vector2)circle.transform.position, line.pos2))
                    {
                        closestEndPoint = line.pos2;
                    }
                    else
                    {
                        closestEndPoint = line.pos1;
                    }

                    //check if collision point is outside of line segment length
                    Vector2 collisionPos = (Vector2)circle.transform.position + (-n * distance);
                    if(Vector2.Distance(collisionPos, line.posMid) > Vector2.Distance(closestEndPoint, line.posMid))
                    {
                        //if outside line seg, check collision towards the closest endpoint,
                        float distanceToPoint = Vector2.Distance((Vector2)circle.transform.position, closestEndPoint);
                        float penetration = circle.radius - distanceToPoint;
                        if(penetration > 0)
                        {
                            Vector2 colN = ((Vector2)circle.transform.position - closestEndPoint).normalized;
                            OverlapCorrection(circleRB, lineRB, penetration, n);
                            if (Vector2.Dot(circleRB.velocity, colN) < 0)
                            {
                                CircleLineCollision(closestEndPoint, colN, circleRB);
                            }
                            
                        }
                    }
                    else
                    {
                        //inside of line segment check with the collision point/line normal
                        float penetration = circle.radius - distance;
                        if (penetration > 0 && distance > 0)
                        {
                            OverlapCorrection(circleRB, lineRB, penetration, n);
                            if(Vector2.Dot(circleRB.velocity, n) < 0)
                            {
                                CircleLineCollision(collisionPos, n, circleRB);
                            }
                        }
                    }
                }
            }
        }
    }

    void OverlapCorrection(eduRigidBody body1, eduRigidBody body2, float penetration, Vector2 colNormal)
    {
        float ERP = 0.2f;

        if(float.IsInfinity(body1.mass)) // static object, either line or plane, only move the circle which is body 2
        {
            body2.transform.position += (Vector3)(penetration * ERP * colNormal);
        }
        else if (float.IsInfinity(body2.mass)) // static object, either line or plane, only move the circle which is body 1
        {
            body1.transform.position += (Vector3)(penetration * ERP * colNormal);
        }
        else
        {
            float Pn = ERP * (body1.mass * body2.mass) / (body1.mass + body2.mass) * penetration;

            body1.transform.position += (Vector3)(-(Pn / body1.mass) * colNormal);
            body2.transform.position += (Vector3)((Pn / body2.mass) * colNormal);
        }
    }

    void CirclePlaneCollision(eduRigidBody circle, Vector2 lineNormal)
    {
        float Cr = circle.cor;
        Vector2 J = -circle.mass * (1 + Cr) * (Vector2.Dot(circle.velocity, lineNormal)) * lineNormal;
        circle.ApplyImpulse(J/circle.mass);
    }

    void CircleCircleCollision(eduRigidBody body1, eduRigidBody body2, Vector2 vRel, Vector2 n)
    {
        float Cr = Mathf.Min(body1.cor, body2.cor);
        float VrelN = Vector2.Dot(vRel, n);

        if (VrelN > 0)
            return;

        float J = (body1.mass * body2.mass) / (body1.mass + body2.mass) * (1 + Cr) * VrelN;

        Vector2 body1Impulse = (J / body1.mass) * n;
        Vector2 body2Impulse = -(J / body2.mass) * n;

        body1.ApplyImpulse(body1Impulse);
        body2.ApplyImpulse(body2Impulse);
    }

    void CircleLineCollision(Vector2 colPos, Vector2 n, eduRigidBody circle)
    {
        float Cr = circle.cor;
        Vector2 J = -circle.mass * (1 + Cr) * (Vector2.Dot(circle.velocity, n)) * n;
        circle.ApplyImpulse(J/circle.mass);
    }
}

