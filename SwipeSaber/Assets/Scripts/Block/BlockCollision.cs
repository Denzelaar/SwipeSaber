using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollision : MonoBehaviour
{
    Direction blockDirection;

    Vector2 firstCollisionPoint;
    Vector2 secondCollisionPoint;
    Vector2 thirdCollisionPoint;

    // Start is called before the first frame update
    public void Init(Direction blockDirection)
    {
        this.blockDirection = blockDirection;
    }

    Direction CheckCollisionDirection(Vector2 pos)
    {
        Direction direction = Direction.Null;

        if (firstCollisionPoint == Vector2.zero)
        {
            firstCollisionPoint = pos;
        }
        else 
        {
            secondCollisionPoint = pos;

            Vector2 dragVectorDirection = (secondCollisionPoint - firstCollisionPoint).normalized;
           
            
            direction =  CalculateDirection(dragVectorDirection);

            secondCollisionPoint = firstCollisionPoint;
            firstCollisionPoint = Vector2.zero;
        }

        return direction;

        //Clear collision points
    }

    Direction CalculateDirection(Vector2 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        Direction collisionDir;

        if (positiveX > positiveY)
        {
            collisionDir = (dragVector.x > 0) ? Direction.Right : Direction.Left;
        }
        else
        {
            collisionDir = (dragVector.y > 0) ? Direction.Up : Direction.Down;
        }

        return collisionDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SwipeTrail"))
        {
            Direction swipeDirection = CheckCollisionDirection(collision.transform.position);

            if(swipeDirection != Direction.Null)
            {
                if(swipeDirection == blockDirection)
                {
                    gameObject.GetComponent<Block>().Hit(true);
                }
                else
                {
                    gameObject.GetComponent<Block>().WrongDirectionHit();
                }
            }
        }
        else if(collision.CompareTag("UpperBorder"))
        {
            gameObject.GetComponent<Block>().Hit(false);
        }
    
    }
}
