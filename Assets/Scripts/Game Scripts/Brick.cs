using Unity.VisualScripting;
using UnityEngine;

public class Brick : MonoBehaviour
{
    enum BrickType
    {
        Normal,
        Explosive
    }

    [SerializeField] SpriteRenderer sprite;

    BrickType type;
    bool isAlive;
    int hp;

    void Start()
    {
        isAlive = true;
        DecideTypeOfBrick();
        SetColor();
    }

    void DecideTypeOfBrick()
    {
        if (Random.Range(0.0f, 1.0f) < 0.95f)
            type = BrickType.Normal;
        else
            type = BrickType.Explosive;

        if (type == BrickType.Normal)
            hp = Random.Range(1, 4);
        else
            hp = 1;
    }

    public void BrickHit()
    {
        hp -= 1;
        if(hp <= 0)
        {
            OnDeath();
            return;
        }

        SetColor();
    }

    void SetColor()
    {
        if (hp == 1)
            sprite.color = Color.green;
        else if (hp == 2)
            sprite.color = Color.yellow;
        else if (hp == 3)
            sprite.color = Color.red;
    }

    void OnDeath()
    {
        isAlive = false;
        if(type == BrickType.Explosive)
        {
            //kaboom
        }
    }

    void Update()
    {
        
    }
}
