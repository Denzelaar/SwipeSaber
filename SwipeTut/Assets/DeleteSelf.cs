using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelf : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCor(float sec)
    {    
        StartCoroutine(DeleteSelfAfterWait(sec));

    }

    IEnumerator DeleteSelfAfterWait(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }
}
