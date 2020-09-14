using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollision : Collision
{
    bool hit = false;

    // Start is called before the first frame update
    public void Init(Direction blockDirection)
    {
        this.blockDirection = blockDirection;
        hit = false;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Collider2D>().enabled)
        {
            if (collision.CompareTag("SwipeTrail") && !hit)
            {
                Direction swipeDirection = CheckCollisionDirection(collision.transform.position);

                if (swipeDirection != Direction.Null)
                {
                    if (swipeDirection == blockDirection)
                    {
                        gameObject.GetComponent<Block>().Hit(true);
                        hit = true;
                    }
                    else
                    {
                        gameObject.GetComponent<Block>().WrongDirectionHit();
                    }
                }
            }
            else if (collision.CompareTag("UpperBorder"))
            {
                gameObject.GetComponent<Block>().Hit(false);
            }
        }
    }
}
