using System.Collections;
using UnityEngine;

public class eduExplosionPoint : MonoBehaviour
{
    [SerializeField] Vector2 pos;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionPower;

    float timer = 0;
    [SerializeField] float explosionTimer = 0; //time between each explosion

    [SerializeField] GameObject explosionCircle; //visual


    void Start()
    {
        pos = transform.position;
        explosionRadius = transform.localScale.x;
        explosionCircle.gameObject.SetActive(false);
    }

    public void UpdateExplosions(eduRigidBody[] RBs) //called from eduForces
    {
        timer += Time.deltaTime;
        if (timer >= explosionTimer)
        {
            Debug.Log("explosion");
            foreach(eduRigidBody rb in RBs)
            {
                ApplyExplosionForce(rb);
            }

            timer = 0;
        }
    }

    void ApplyExplosionForce(eduRigidBody rb)
    {
        float distance = Vector2.Distance(pos, rb.transform.position);
        if (distance < explosionRadius) //within explosion
        {
            Vector2 direction = ((Vector2)rb.transform.position - pos).normalized;

            float powerFalloff = 1 - (distance / explosionRadius);

            Vector2 impulse = direction * explosionPower / rb.mass;
            rb.ApplyImpulse(impulse);
            StartCoroutine(MakeExplosionVisible());
        }
    }

    IEnumerator MakeExplosionVisible()
    {
        explosionCircle.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        explosionCircle.gameObject.SetActive(false);
    }
}
