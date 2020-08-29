using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeHandler : MonoBehaviour
{
    SwipeManager swipeManager;
    public GameObject prefab;
    public Transform trailParent;
    public float trailSpacing;
    public Vector2 newPos;
    public Text text;


    // Start is called before the first frame update
    void Start()
    {
        swipeManager = GetComponent<SwipeManager>();
        swipeManager.onSwipePos += HandleSwipe;

    }

    private void OnDisable()
    {
        swipeManager.onSwipePos -= HandleSwipe;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject henk = Instantiate(prefab, trailParent);
            henk.GetComponent<RectTransform>().localPosition = newPos;
        }
    }

    public void HandleSwipe(Vector2 start, Vector2 end)
    {
        Debug.Log("SWIPPERRE");
        Debug.Log(start + " " + end);
        CreateTrail(start, end);
    }

    private void CreateTrail(Vector2 start, Vector2 end)
    {
        //Creates a trail along the current finished swipe
        var distance = Vector2.Distance(end, start);

        var amountOfObjects = (int) (distance / trailSpacing);
        Debug.Log(start + " dfsd" + end);

        Debug.Log("Distance: " + distance + " Amount of Objects " + amountOfObjects);

        Vector2 direction = (end - start).normalized;
            
        GameObject[] trailObjects = new GameObject[amountOfObjects];

        text.text = "start " + start + " End " + end;


        for (int i = 0; i < trailObjects.Length; i++)
        {
            trailObjects[i] = (GameObject)Instantiate(prefab, trailParent); 
            float newSpacing = trailSpacing * (i + 1);

            RectTransform rect = trailObjects[i].GetComponent<RectTransform>(); 
             rect.localPosition = start + (direction * newSpacing);

            trailObjects[i].GetComponent<DeleteSelf>().seconds = 1 + (i / 500);
        }

    }
}
