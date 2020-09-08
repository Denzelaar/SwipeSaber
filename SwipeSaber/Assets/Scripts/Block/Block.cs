﻿using System.Collections;
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
    public delegate void OnHit();
    public static OnHit BlockHit;

    public Direction blockDirection; //{ get; private set; }
    public bool hit;
    public GameObject obstacleChild;
    public ObstacleType obstacleType;

    BlockCollision bc;
    BlockMovement bm;
    Transform arrowTransform;

    SpriteRenderer spriteRenderer;
    SpriteRenderer obstacleChildRenderer;

    Collider2D obstacleChildCollider;


    public Color spriteColor;

    // Start is called before the first frame update
    private void Start()
    {
        bc = GetComponent<BlockCollision>();
        bm = GetComponent<BlockMovement>();
        arrowTransform = transform.GetChild(0).GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;

        obstacleChildRenderer = obstacleChild.GetComponent<SpriteRenderer>();
        obstacleChildCollider = obstacleChild.GetComponent<Collider2D>();
    }

    public void Init(Direction bd, float speed, bool moving, ObstacleType obstacleType = ObstacleType.Null)
    {
        if(bc == null)
        {
            Start();
        }

        blockDirection = bd;       
        bc.Init(blockDirection);
        bm.Init(speed, moving);
        RotateSprite(bd);

        if (hit)
        {
            ChangeColorBack();
        }

        this.obstacleType = obstacleType;

        if (obstacleType != ObstacleType.Null)
        {
            if(obstacleType == ObstacleType.Shield)
            {
                //change child to shield, activate collider
                obstacleChildRenderer.sprite = Resources.Load();
                obstacleChildRenderer.enabled = true;

                obstacleChildCollider.enabled = true;
            }
            else if(obstacleType == ObstacleType.Slime)
            {
                //change child to slime, activate sprite
                obstacleChildRenderer.sprite = Resources.Load();
                obstacleChildRenderer.enabled = true;

                if (obstacleChildCollider.enabled)
                {
                    obstacleChildCollider.enabled = false;
                }
            }
            else if(obstacleType == ObstacleType.Spike)
            {
                //change child to spike, deactivate spriterenderer
                obstacleChildRenderer.sprite = Resources.Load();
                obstacleChildRenderer.enabled = true;

                obstacleChildCollider.enabled = true;
            }
            obstacleChild.SetActive(true);
        }
        else
        {
            //check if spriteRenderer is active
            if (obstacleChild.activeSelf)
            {
                obstacleChild.SetActive(false);
            }
        }
    }

    public void Hit(bool score)
    {
        if (score)
        {
            BlockHit();
        }
        gameObject.SetActive(false);
    }

    public void WrongDirectionHit()
    {
        hit = true;
        spriteRenderer.color = Color.red;

        if (gameObject.activeSelf)
        {
            StartCoroutine(ChangeColor(.5f));
        }
    }

    IEnumerator ChangeColor(float sec)
    {
        yield return new WaitForSeconds(sec);
        ChangeColorBack();
    }

    void ChangeColorBack()
    {
        spriteRenderer.color = spriteColor;
        hit = false;
    }

    void RotateSprite(Direction dir)
    {
        arrowTransform.rotation = Quaternion.identity;

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
