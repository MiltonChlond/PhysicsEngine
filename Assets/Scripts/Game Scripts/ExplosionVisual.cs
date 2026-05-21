using System.Threading;
using UnityEngine;

public class ExplosionVisual : MonoBehaviour
{

    float timer = 0;
    float threshold = 0.05f;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= threshold)
        {
            Destroy(this.gameObject);
        }
    }
}
