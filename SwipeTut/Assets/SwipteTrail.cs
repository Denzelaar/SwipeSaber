using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipteTrail : MonoBehaviour
{
    public GameObject trailPrefab;

    Plane planeObj;
    GameObject thisTrail;
    Vector3 startPos;

    private void Start()
    {
        planeObj = new Plane(Camera.main.transform.forward * -1, this.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            || Input.GetMouseButtonDown(0))
        {
            Ray mray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (planeObj.Raycast(mray, out rayDistance))
            {
                startPos = mray.GetPoint(rayDistance);
            }

            thisTrail = (GameObject)Instantiate(trailPrefab, startPos,
Quaternion.identity);

        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            || Input.GetMouseButton(0))
        {
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;

            if (planeObj.Raycast(mRay, out rayDistance))
            {
                thisTrail.transform.position = mRay.GetPoint(rayDistance);
            }
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                    || Input.GetMouseButtonUp(0))
        {
            if (Vector3.Distance(thisTrail.transform.position, startPos) < .1f)
            {
                Destroy(thisTrail);
            }
        }
    }
}
