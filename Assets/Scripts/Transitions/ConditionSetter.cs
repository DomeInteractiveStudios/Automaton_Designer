using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionSetter : MonoBehaviour
{
    public List<string> list; //list of conditions
    private GameObject trans; //transition obj || end state obj
    private Transform state; 
    public void SetConditions()
    {
        TransitionScript transScript = trans.GetComponent<TransitionScript>(); 

        /*COPY EVERY CONDTION FROM THE LIST TO THE ARRAY*/
        transScript.conditions = new string[list.Count];
        for(int i = 0; i < list.Count; i++)
        {
            transScript.conditions[i] = list[i];
        }
    }
    
    public void GetConditions(string cond)
    {
        list.Add(cond); 
    }

    public void GetTransition(GameObject newTransition)
    {
        trans = newTransition; 
        //if the list is not empty -> clear it
        if(list.Count > 0) list.Clear(); 
    }
}
