using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditState : MonoBehaviour
{
    private StateGenerator stateGenerator;
    [HideInInspector] public GameObject EditStateOptions; 
    private bool canEdit = false; 

    private void Awake() //FIND THE STATE GENERATOR SCRIPT
    {
        stateGenerator = GameObject.Find("ScriptHolder").GetComponent<StateGenerator>();
        EditStateOptions = transform.Find("EditStateOptions").gameObject;

        EditStateOptions.SetActive(false);
    }

    private void Update()
    {
        /*THIS GENERATES THE EDIT STATE BUTTON*/
        if (Input.GetKeyDown(KeyCode.Mouse1) && canEdit)
        {
            EditStateOptions.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && !canEdit)
        {
            EditStateOptions.SetActive(false);
        }
    }

    public void OnStateSprite() //ON MOUSE HOVER
    {
        stateGenerator.canCreate = false;
        canEdit = true;
    }

    public void OffStateSprite() //ON MOUSE EXIT
    {
        stateGenerator.canCreate = true;
        canEdit = false;
        stateGenerator.stateToEdit = null;
        stateGenerator.status = 0;
    }

    public void OpenEditStateOptions() //THIS EVENT IS TRIGGERED BY THE EDIT STATE BUTTON
    {
        stateGenerator.ShowPopUpWindow();
        stateGenerator.status = 1; 
        stateGenerator.stateToEdit = gameObject;
        EditStateOptions.SetActive(false);
    }
}
