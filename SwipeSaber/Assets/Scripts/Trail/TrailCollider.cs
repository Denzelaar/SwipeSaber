using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Block.ShieldHit += DeactivateCollider;
    }

    private void OnDestroy()
    {
        Block.ShieldHit -= DeactivateCollider;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }


    void DeactivateCollider()
    {
        gameObject.SetActive(false);
    }
}
