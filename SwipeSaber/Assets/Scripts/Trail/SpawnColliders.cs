using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnColliders : MonoBehaviour
{
    public TrailRenderer trail; //the trail
    public GameObject trailFollower;
    public GameObject colliderPrefab;
    public Transform colliderParent;

    public int poolSize = 5;
    public GameObject[] pool;
    public List<GameObject> poolList;

    public void Init(Transform colParent)
    {
        trail = GetComponent<TrailRenderer>();
        pool = new GameObject[poolSize];
        poolList = new List<GameObject>(poolSize);    
        colliderParent = colParent;

        for (int i = 0; i < poolSize; i++)
        {
            poolList.Add(Instantiate(colliderPrefab, colliderParent));
            Debug.Log("Insta " + i);
        }
    }

    void FixedUpdate()
    {
        if (!trail.isVisible)
        {
            Debug.Log("Not Visible");
            for (int i = 0; i < poolList.Count; i++)
            {
                poolList[i].gameObject.SetActive(false);

            }
            trail.gameObject.SetActive(false);
        }
        else
        {
            TrailCollission();
        }      
        
    }

    void TrailCollission()
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if (poolList[i].gameObject.activeSelf == false)
            {
                poolList[i].transform.localScale = new Vector3(trail.startWidth, trail.startWidth, 1);
                poolList[i].gameObject.SetActive(true);
                poolList[i].gameObject.transform.position = gameObject.transform.position;
                StartCoroutine(DisableAfterTime(trail.time, poolList[i]));
                return;
            }
        }       

    }

    private IEnumerator DisableAfterTime(float sec, GameObject toDisable)
    {
        yield return new WaitForSeconds(sec);
        toDisable.SetActive(false);
    }

    public void ClearPool()
    {
        int length = poolList.Count;

        for (int i = 0; i < length; i++)
        {
            Destroy(poolList[i]);
            Debug.Log("Destroyed " + i + " out of: " + poolList.Count);
        }

    }

    private void OnDestroy()
    {
        foreach (var item in poolList)
        {
            Destroy(item);
        }
    }

}
