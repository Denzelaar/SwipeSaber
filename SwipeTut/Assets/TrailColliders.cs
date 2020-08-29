using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailColliders : MonoBehaviour
{
    public TrailRenderer trail; //the trail
    public GameObject TrailFollower;
    public GameObject ColliderPrefab;

    public int poolSize = 5;
    GameObject[] pool;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(ColliderPrefab);
        }
    }

    void Update()
    {
        if (!trail.isVisible)
        {            
            for (int i = 0; i < pool.Length; i++)
            {
                pool[i].gameObject.SetActive(false);

            }
        }
        else
        {
            TrailCollission();
        }

    }

    void TrailCollission()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].gameObject.activeSelf == false)
            {
                pool[i].gameObject.SetActive(true);
                pool[i].gameObject.transform.position = gameObject.transform.position;
                return;
            }
        }
    }

    private void OnDestroy()
    {

        foreach (var item in pool)
        {
            Destroy(item);
        }
    }
}
