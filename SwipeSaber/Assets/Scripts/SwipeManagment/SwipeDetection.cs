using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeDetection : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject prefab;
    public Transform canvas;
    public Text text;

    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    private void Update()
    {
        foreach (Touch  touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {

                GameObject henk = (GameObject)Instantiate(prefab, canvas);
                henk.transform.position = touch.position;
                text.text = ("" + touch.position);

            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Press position + " + eventData.pressPosition);
        
        GameObject henk =  (GameObject)Instantiate(prefab, canvas);
        henk.transform.position = eventData.pressPosition;

        Debug.Log("End position + " + eventData.position);
        GameObject henkEnd = (GameObject)Instantiate(prefab, canvas);
        henkEnd.transform.position = eventData.pressPosition;

        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        Debug.Log("norm + " + dragVectorDirection);
        GetDragDirection(dragVectorDirection);
    }

    //It must be implemented otherwise IEndDragHandler won't work 
    public void OnDrag(PointerEventData eventData)
    {

    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        Debug.Log(draggedDir);
        return draggedDir;
    }
}
