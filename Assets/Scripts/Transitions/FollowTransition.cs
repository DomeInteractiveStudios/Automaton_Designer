using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransition : MonoBehaviour
{
    /*SCRIPT ATTACHED TO THE TEXT GAMEOBJECT MAKING THE TEXT FOLLOW THE TRANSITION LINE*/
    private TransitionScript transitionScript; // Reference to the TransitionScript component.
    [SerializeField] private Vector2 offset; //offset of the text from the middle of the transition line

    private void Awake()
    {
        transitionScript = transform.parent.GetComponent<TransitionScript>(); // Get the TransitionScript component from the parent object
    }

    private void Update()
    {
        if(transitionScript.isAuto) offset = new Vector2(0, -0.8f); //if the transition is an auto transition, set the offset is fixed
    }
    private void LateUpdate()
    {
        if(!transitionScript.goUnder) transform.position = new Vector3(transitionScript.states[1].transform.position.x + offset.x, transitionScript.states[1].transform.position.y + offset.y, 0); //set the position of the text at the middle point of the transition line
        else transform.position = new Vector3(transitionScript.states[1].transform.position.x + offset.x, transitionScript.states[1].transform.position.y - offset.y, 0); //set the position of the text at the middle point of the transition line
    }
}
