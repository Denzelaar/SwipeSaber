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
    public delegate void OnHit();
    public static OnHit BlockHit;
    public static OnHit ShieldHit;

    public Direction blockDirection; //{ get; private set; }

    public bool wrongHit;

    public GameObject spike;
    public GameObject shield;

    public Transform arrowTransform;

    public ObstacleType obstacleType;

    public Color spriteColor;

    BlockCollision bc;
    BlockMovement bm;

    SpriteRenderer spriteRenderer;
    SpriteRenderer obstacleChildRenderer;

    Collider2D obstacleChildCollider;
    Collider2D collider;

    Dissolve dissolve;
    Dissolve arrowDissolve;

    ShieldCollision shieldCollision;



    // Start is called before the first frame update
    private void Start()
    {
        bc = GetComponent<BlockCollision>();
        bm = GetComponent<BlockMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;

        dissolve = GetComponent<Dissolve>();
        arrowDissolve = arrowTransform.gameObject.GetComponent<Dissolve>();

        obstacleChildRenderer = spike.GetComponent<SpriteRenderer>();
        obstacleChildCollider = spike.GetComponent<Collider2D>();
        collider = GetComponent<Collider2D>();

    }

    public void Init(Direction bd, float speed, bool moving, ObstacleType obstacleType = ObstacleType.Null)
    {
        if (bc == null)
        {
            Start();
        }

        blockDirection = bd;       
        bc.Init(blockDirection);
        bm.Init(speed, moving);
        RotateSprite(bd);

        if (wrongHit)
        {
            wrongHit = false;
            ChangeColorBack();
        }

        if (!arrowTransform.gameObject.activeSelf)
        {
            arrowTransform.gameObject.SetActive(true);
        }

        this.obstacleType = obstacleType;

        CheckObstacleType();      
    }

    public void OnShieldHit()
    {
        Debug.Log("in on hit");
 

        collider.enabled = true;
        bc.enabled = true;
    }

    public void Hit(bool score)
    {
        if (score)
        {
            StartDissolve();
            BlockHit();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void WrongDirectionHit()
    {
        wrongHit = true;
        spriteRenderer.color = Color.red;

        if (gameObject.activeSelf)
        {
            StartCoroutine(ChangeColor(.5f));
        }
    }

    void StartDissolve()
    {
        float random = Random.Range(10.0f, 100.0f);

        dissolve.StartDissolve(random);
        arrowDissolve.StartDissolve(random);

    }


    void CheckObstacleType()
    {
        if (obstacleType != ObstacleType.Null)
        {
            if (obstacleType == ObstacleType.Shield)
            {
                //change child to shield, activate collider
                //obstacleChildRenderer.sprite = Resources.Load();
                shield.SetActive(true);
                collider.enabled = false;
                bc.enabled = true;
            }
            else if (obstacleType == ObstacleType.Slime)
            {
                //change child to slime, activate sprite
                //obstacleChildRenderer.sprite = Resources.Load();
                obstacleChildRenderer.enabled = true;

                if (obstacleChildCollider.enabled)
                {
                    obstacleChildCollider.enabled = false;
                }
            }
            else if (obstacleType == ObstacleType.Spike)
            {
                //change child to spike, deactivate spriterenderer
                //obstacleChildRenderer.sprite = Resources.Load();
                obstacleChildRenderer.enabled = true;

                obstacleChildCollider.enabled = true;
                spike.SetActive(true);
            }
        }
        else
        {
            //check if spriteRenderer is active
            if (spike.activeSelf)
            {
                //obstacleChild.SetActive(false);
            }
            if (!collider.enabled)
            {
                //collider.enabled = true;
            }
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
        wrongHit = false;
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
