using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public float blockSpeed;
    public bool playing;
    bool is2D = true;

    // Start is called before the first frame update
    public void Init(float speed, bool isPlaying)
    {
        Rigidbody2D try2D;
        TryGetComponent<Rigidbody2D>(out try2D);

        if (try2D == null)
        {
            is2D = false;
        }

        blockSpeed = speed;
        playing = isPlaying;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            if (is2D)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, blockSpeed);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = new Vector2(0, blockSpeed);

            }
        }
    }

    void Pauze()
    {
        playing = true;
    }

    IEnumerator DestroyBlock(float timeIsSeconds)
    {
        yield return new WaitForSeconds(timeIsSeconds);
        Destroy(gameObject);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + " " + gameObject.name);
        if (collision.gameObject.tag == "UpperBorder")
        {
            StartCoroutine(DestroyBlock(.2f));
        }
        else if(collision.gameObject.tag == "SwipeTrail")
        {
            StartCoroutine(DestroyBlock(.2f));
        }
    }*/    
}
