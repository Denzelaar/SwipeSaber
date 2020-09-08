using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollision : MonoBehaviour
{
    Direction blockDirection;

    Vector2 firstCollisionPoint;
    Vector2 secondCollisionPoint;
    Vector2 thirdCollisionPoint;

    Direction lastDirection;

    bool hit = false;

    // Start is called before the first frame update
    public void Init(Direction blockDirection)
    {
        this.blockDirection = blockDirection;
        hit = false;
    }

    Direction CheckCollisionDirection(Vector2 pos)
    {
        //three points, check if new point is to left, right, above or below last point, check if their is 2 last points,
        //if true check if this point has same direction from last point as last point had to point before it.
        //if true return direction, if false return false
        //lastly set point 2 to point 1 and point 3 to point 2

        Direction direction = Direction.Null;

        if (firstCollisionPoint == Vector2.zero)
        {
            firstCollisionPoint = pos;
        }
        else if(secondCollisionPoint == Vector2.zero)
        {
            secondCollisionPoint = pos;

            Vector2 dragVectorDirection = (secondCollisionPoint - firstCollisionPoint).normalized;

            lastDirection =  CalculateDirection(dragVectorDirection);
        }
        else
        {
            thirdCollisionPoint = pos;

            Vector2 dragVectorDirection = (thirdCollisionPoint - secondCollisionPoint).normalized;

            Direction newDirection = CalculateDirection(dragVectorDirection);

            if(lastDirection == newDirection)
            {
                direction = newDirection;
            }

            firstCollisionPoint = secondCollisionPoint;
            secondCollisionPoint = thirdCollisionPoint;
            lastDirection = newDirection;
            Debug.Log(newDirection);
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
        if (collision.CompareTag("SwipeTrail") && !hit)
        {
            Direction swipeDirection = CheckCollisionDirection(collision.transform.position);

            if(swipeDirection != Direction.Null)
            {
                if(swipeDirection == blockDirection)
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
        else if(collision.CompareTag("UpperBorder"))
        {
            gameObject.GetComponent<Block>().Hit(false);
        }
    
    }
}
