using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryTransition : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject pointerFollower, stateHolder; 
    private Transform[] points = new Transform[2];
    private bool isDragging; 
    private RaycastHit2D hit; //raycast hit
    [HideInInspector] public bool isSet = false; //is the transition set?
    [HideInInspector] public GameObject firstState; //first state of the transition

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        points[0] = this.transform; // Start point
        pointerFollower = transform.GetChild(0).gameObject; // Invisible object that follows the mouse pointer
    }

    private void Start()
    {
        stateHolder = GameObject.FindWithTag("StateHolder"); // Get the StateHolder gameObject
    }

    private void Update()
    {
        if(points[1] != null)
        {
            lineRenderer.SetPosition(0, points[0].position);
            lineRenderer.SetPosition(1, points[1].position);
        }

        if(isDragging)
        {
            pointerFollower.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if(Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0),  Vector2.zero);
            
                if (hit.collider != null)
                {
                    SetEnd(hit.collider.transform); // Set the end position of the line renderer
                    isSet = true; // Set the isSet variable to true
                }
                else SetEnd(transform);
            }
        }
    }
    public void StartSet()
    {
        if(stateHolder.transform.childCount > 0)
        {
            isSet = false; // Reset the isSet variable
            points[1] = pointerFollower.transform;
            isDragging = true;
            //UnityEngine.Debug.Log("Start Set triggered: " + isDragging);
        }
    }
    private void SetEnd(Transform end)
    {
        points[1] = end;
        firstState = points[1].gameObject;
        isDragging = false;
    }
}