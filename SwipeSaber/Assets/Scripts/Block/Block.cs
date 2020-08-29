using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    Up,
    Right,
    Down,
    Left,
    Null
}

public class Block : MonoBehaviour
{
    public Direction blockDirection; //{ get; private set; }

    BlockCollision bc;
    BlockMovement bm;
    Transform arrowTransform;

    SpriteRenderer spriteRenderer;

    Color spriteColor;

    // Start is called before the first frame update
    public void Init(Direction bd, float speed, bool moving)
    {
        blockDirection = bd;
        bc = GetComponent<BlockCollision>();
        bm = GetComponent<BlockMovement>();
        arrowTransform = transform.GetChild(0).GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        bc.Init(blockDirection);
        bm.Init(speed, moving);
        RotateSprite(bd);
        spriteColor = spriteRenderer.color;
    }

    public void Hit()
    {
        gameObject.SetActive(false);
    }

    public void WrongDirectionHit()
    {
        spriteRenderer.color = Color.red;
        StartCoroutine(ChangeColor(.5f));
    }

    IEnumerator ChangeColor(float sec)
    {
        yield return new WaitForSeconds(sec);
        spriteRenderer.color = spriteColor;
    }

    void RotateSprite(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                arrowTransform.Rotate(new Vector3(0,0,270));
                break;
            case Direction.Right:
                arrowTransform.Rotate(new Vector3(0, 0, 180));
                break;
            case Direction.Down:
                arrowTransform.Rotate(new Vector3(0, 0, 90));
                break;
            case Direction.Left:
                //Sprite default is left
                break;
            default:
                break;
        }
    }
}
