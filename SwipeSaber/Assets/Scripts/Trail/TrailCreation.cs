﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCreation : MonoBehaviour
{
    public GameObject trailPrefab;
    public Transform trailParent;
    public Transform colParent;

    Plane planeObj0;
    Plane planeObj1;
    GameObject trail0;
    GameObject trail1;
    Vector3 startPos;

    bool creatingTrail0 = false;
    bool creatingTrail1 = false;

    private void Start()
    {
        planeObj0 = new Plane(Camera.main.transform.forward * -1, this.transform.position);
        planeObj1 = new Plane(Camera.main.transform.forward * -1, this.transform.position);

        TrailDestroyed.trailIsDestroyed += TrailHasBeenDestroyed;
    }

    // Update is called once per frame
    void Update()
    {
        //First Trail
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        || Input.GetMouseButtonDown(0))
        {
            if (!creatingTrail0)
            {
                creatingTrail0 = true;
                trail0 = (GameObject)Instantiate(trailPrefab, trailParent);
                trail0.name = "trail0";

                trail0.SetActive(false);

                if (Application.isEditor)
                {
                    creatingTrail0 = BeganTouchPhase(trail0, planeObj0, Input.mousePosition);
                }
                else
                {
                    creatingTrail0 = BeganTouchPhase(trail0, planeObj0, Input.GetTouch(0).position);
                }
            }
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            || Input.GetMouseButton(0))
        {
            if (Application.isEditor)
            {
                creatingTrail0 = BeganTouchPhase(trail0, planeObj0, Input.mousePosition);
            }
            else
            {
                creatingTrail0 = BeganTouchPhase(trail0, planeObj0, Input.GetTouch(0).position);
            }
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                    || Input.GetMouseButtonUp(0))
        {
            EndTouchPhase(trail0, creatingTrail0);
        }        

        //Second Trail
        if ((Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
        || Input.GetMouseButtonDown(1))
        {
            if (!creatingTrail1)
            {
                creatingTrail1 = true;
                trail1 = (GameObject)Instantiate(trailPrefab, trailParent);
                trail1.name = "trail1";

                trail1.SetActive(false);
                creatingTrail1 = BeganTouchPhase(trail1, planeObj1, Input.GetTouch(1).position);

            }
        }
        else if ((Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Moved)
            || Input.GetMouseButton(1))
        {
            MovedTouchPhase(trail1, planeObj1, Input.GetTouch(1).position);
        }
        else if ((Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Ended)
                    || Input.GetMouseButtonUp(1))
        {
            EndTouchPhase(trail1, creatingTrail1);
        }                    
    }

    void TrailHasBeenDestroyed(string objectName)
    {
        if(objectName == "trail0")
        {
            creatingTrail0 = false;
        }
        else if (objectName == "trail1")
        {
            creatingTrail1 = false;
        }
    }

    private bool BeganTouchPhase(GameObject trail, Plane plane, Vector3 pos)
    {
        if(trail != null)
        {
            Ray mray = Camera.main.ScreenPointToRay(pos);

            if (plane.Raycast(mray, out float rayDistance))
            {
                startPos = mray.GetPoint(rayDistance);
            }

            if (!trail.GetComponent<SpawnColliders>().initialized)
            {
                trail.GetComponent<SpawnColliders>().Init(colParent);
            }

            trail.SetActive(true);
            trail.transform.position = startPos;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MovedTouchPhase(GameObject trail, Plane plane, Vector3 pos)
    {
        if (trail != null)
        {
            Ray mRay = Camera.main.ScreenPointToRay(pos);
            float rayDistance;

            if (plane.Raycast(mRay, out rayDistance))
            {
                trail.transform.position = mRay.GetPoint(rayDistance);
            }
        }
    }

    private void EndTouchPhase(GameObject trail, bool trailBool)
    {
        if(trail != null)
        {
            if (Vector3.Distance(trail.transform.position, startPos) < .1f)
            {
                //trail.GetComponent<SpawnColliders>().ClearPool();
                //Destroy(trail);
                trailBool = false;
            }
        }
      
    }
}
