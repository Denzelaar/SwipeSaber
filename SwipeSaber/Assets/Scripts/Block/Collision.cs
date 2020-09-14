using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public Direction blockDirection;
    public Direction lastDirection;

    public Vector2 firstCollisionPoint;
    public Vector2 secondCollisionPoint;
    public Vector2 thirdCollisionPoint;

    public Direction CheckCollisionDirection(Vector2 pos)
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
        else if (secondCollisionPoint == Vector2.zero)
        {
            secondCollisionPoint = pos;

            Vector2 dragVectorDirection = (secondCollisionPoint - firstCollisionPoint).normalized;

            lastDirection = CalculateDirection(dragVectorDirection);
        }
        else
        {
            thirdCollisionPoint = pos;

            Vector2 dragVectorDirection = (thirdCollisionPoint - secondCollisionPoint).normalized;

            Direction newDirection = CalculateDirection(dragVectorDirection);

            if (lastDirection == newDirection)
            {
                direction = newDirection;
            }

            firstCollisionPoint = secondCollisionPoint;
            secondCollisionPoint = thirdCollisionPoint;
            lastDirection = newDirection;
        }

        return direction;

        //Clear collision points
    }

    public Direction CalculateDirection(Vector2 dragVector)
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

}
