using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailDestroyed : MonoBehaviour
{
    public delegate void OnTrailDestroyed(string trailName);
    public static OnTrailDestroyed trailIsDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        trailIsDestroyed(gameObject.name);
    }
}
