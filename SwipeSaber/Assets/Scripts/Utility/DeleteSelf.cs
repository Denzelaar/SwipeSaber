using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelf : MonoBehaviour
{
    public float seconds = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteAfterWait(seconds));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DeleteAfterWait(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);

    }
}
