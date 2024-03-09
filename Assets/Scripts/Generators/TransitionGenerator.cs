using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionGenerator : MonoBehaviour
{
    [HideInInspector] public Transform end; //state at the end of the transition
    private TransitionScript transitionScript; 
    private  StateGenerator stateGenerator; // Reference to the StateGenerator component.
    private ConditionSetter condSetter; //Reference to the Condition Setter Script
    private EditState editState; //edit state script
    private int i=0; //debugging purposes
    private GameObject transitionPrefab, stateHolder, pointerFollower, middlePoint, newTransition; //transition prefab || state parent gameObject || invisible object that follows the mouse pointer || middle point of the line renderer || new transition gameObject created
    //private bool exists = false; //check if the transition has been created
    public bool isDragging = false; //check if the transition is still being associated with an end state
    private RaycastHit2D hit; //raycast hit
    
    private void Awake()
    {
        editState = GetComponent<EditState>();

        stateGenerator = GameObject.Find("ScriptHolder").GetComponent<StateGenerator>(); // Get the StateGenerator script
        condSetter = GameObject.Find("ScriptHolder").GetComponent<ConditionSetter>(); // Get the ConditionSetter script
        stateHolder = stateGenerator.stateHolder; // Get the stateHolder GameObject from the StateGenerator script
    }

    private void Update()
    {
        if(isDragging)
        {
            transitionScript.states[0] = transform.gameObject; //set the start position of the line renderer
            transitionScript.states[1] = middlePoint; //set the middle position of the line renderer
            transitionScript.states[2] = pointerFollower; //set the end position of the line renderer
            pointerFollower.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0); //set the position of the pointer follower to the mouse position
            transitionScript.midPointOffset = Vector3.Distance(transitionScript.states[0].transform.position, transitionScript.states[2].transform.position) / 5; //set the offset of the middle point from the start and end points

            if(Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0),  Vector2.zero);
            
                if (hit.collider != null) end = hit.collider.transform;
                EndDragging();
            }
        }
    }

    public void CreateTransition()
    {
        /*"CLEAN" END POINT BUFFER*/
        end = null;
        /*if(end == null) UnityEngine.Debug.Log("End is null at the start");
        else UnityEngine.Debug.Log(end.gameObject.name + " is the end state of the transition at first");*/

        /*ALWAYS CREATE TRANSTION GAME OBJECT (LINE RENDERER)*/
        transitionPrefab = Resources.Load<GameObject>("Prefabs/Transition");
        newTransition = Instantiate(transitionPrefab, transform.position, Quaternion.identity); //instantiate the transition
        newTransition.name = $"Transition_{i}"; //set the name of the transition
        newTransition.transform.SetParent(transform); //set the transition as a child of the state
        transitionScript = newTransition.GetComponent<TransitionScript>(); 
        condSetter.GetTransition(newTransition); 
        //for(int i=0; i<transitionScript.states.Length; i++) UnityEngine.Debug.Log(transitionScript.states[i] + " is the state at position " + i);
        editState.EditStateOptions.SetActive(false);

        if(CountStates() < 2) CreateAutoTransition(); //if there are less than 2 states, only an auto transition can be created
        else
        { 
            pointerFollower = newTransition.transform.GetChild(0).gameObject; // Random object that follows the mouse pointer
            middlePoint = newTransition.transform.GetChild(1).gameObject; // Middle point of the line renderer
            isDragging = true;
        }

        UnityEngine.Debug.Log("New transition is not null " + newTransition.name + $" Should be Transition_{i}");
        i++; 
        //if(newTransition != null) newTransition = null; //"clean" the transition buffer
    }

    public void EndDragging()
    {
        isDragging = false;
        /*if(end != null) UnityEngine.Debug.Log(end.gameObject.name + " is the end state of the transition");
        else UnityEngine.Debug.Log("End is null");*/
        if(end == null) transitionScript.DestroyTransition(); //this has to be checked before the second if otherwise the script will not know what end is
        //and will send an error before even reaching the destroy transition function, so it can't be set as the else of the second if

        if(end!=null && hit.collider.tag == "State") transitionScript.states[2] = end.gameObject; //set the end position of the line renderer
        if(transitionScript.states[0] == transitionScript.states[2]) CreateAutoTransition();

        transitionScript.OpenConditionMenu();
    }

    private int CountStates()
    {
        int count = 0;
        foreach(Transform state in stateHolder.transform)
        {
            count++;
        }
        return count;
    }

    private void CreateAutoTransition()
    {
        //Create a transition from a state to itself
        /*REFRENCE POINTS CAN NO LONGER BE THEM SELVES OTHER WISE ALL 3 WOULD BE THE OBJECT*/
        transitionScript.states[0] = transform.Find("AutoTransStart").gameObject;
        transitionScript.states[1] = transform.Find("AutoTransMid").gameObject;
        transitionScript.states[2] = transform.Find("AutoTransEnd").gameObject;

        /*CHANGE THE POSTION OF THE LINE RENDERER POINTS, SO THAT YOU CAN CREATE AN ARCH FROM THE STATE TO ITSELF*/
        transitionScript.states[0].transform.position = new Vector3(transitionScript.states[0].transform.position.x + 0.5f, transitionScript.states[0].transform.position.y, 0);
        transitionScript.states[1].transform.position = new Vector3(transitionScript.states[1].transform.position.x, transitionScript.states[1].transform.position.y + 2.5f, 0);
        transitionScript.states[2].transform.position = new Vector3(transitionScript.states[2].transform.position.x - 0.5f, transitionScript.states[2].transform.position.y, 0);
        transitionScript.isAuto = true;

        transitionScript.OpenConditionMenu();
    }
}
