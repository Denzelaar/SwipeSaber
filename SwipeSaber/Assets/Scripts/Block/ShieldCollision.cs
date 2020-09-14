using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldCollision : Collision
{
    public delegate void OnHit();
    public static OnHit ShieldHit;
    public bool checkShield = false;
    public GameObject col;

    public void Init()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("SwipeTrail"))
        {
            Direction swipeDirection = CheckCollisionDirection(collision.transform.position);

            if (swipeDirection != Direction.Null)
            {
                if (ShieldHit != null)
                {
                    ShieldHit.Invoke();
                }
                StartCoroutine(Disable(1f));

                if (checkShield)
                {
                    Time.timeScale = 0;
                    col = collision.gameObject;

                }
                Debug.Log("Hit " + collision.gameObject.name + transform.parent.gameObject.name);

            }
        }
        else if (collision.CompareTag("UpperBorder"))
        {
            transform.parent.GetComponent<Block>().Hit(false);
        }
    }

    IEnumerator Disable(float sec)
    {
        yield return new WaitForSeconds(sec);
        transform.parent.GetComponent<Block>().OnShieldHit();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Debug.Log("Dis " + transform.parent.gameObject.name);
    }
}
