using UnityEngine;
using UnityEngine.Rendering;

public class eduExplosion
{
    public Vector3 pos;
    public float explosionRadius;
    public float explosionPower;

    public eduExplosion(Vector3 position, float radius, float power)
    {
        this.pos = position;
        this.explosionRadius = radius;
        this.explosionPower = power;
    }
}
