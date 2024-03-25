using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditState : MonoBehaviour
{
    private StateGenerator stateGenerator;
    [HideInInspector] public GameObject EditStateOptions; 
    private bool canEdit = false, menuOpen = false; 
    [SerializeField] private Checks checks; // Reference to the Checks scriptable object.

    private void Awake() //FIND THE STATE GENERATOR SCRIPT
    {
        stateGenerator = GameObject.Find("ScriptHolder").GetComponent<StateGenerator>();
        EditStateOptions = transform.Find("EditStateOptions").gameObject;

        EditStateOptions.SetActive(false);
    }

    private void Update()
    {
        if(checks.canModify)
        {
            /*THIS GENERATES THE EDIT STATE BUTTON*/
            if (Input.GetKeyDown(KeyCode.Mouse1) && canEdit)
            {
                EditStateOptions.SetActive(true);
                menuOpen = true;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && !canEdit)
            {
                EditStateOptions.SetActive(false);
                menuOpen = false;
            }

            /*DELETE STATE*/
            if(menuOpen)
            {
                if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void OnStateSprite() //ON MOUSE HOVER
    {
        if(checks.canModify)
        {
            stateGenerator.canCreate = false;
            canEdit = true;
        }
    }

    public void OffStateSprite() //ON MOUSE EXIT
    {
        if(checks.canModify)
        {
            stateGenerator.canCreate = true;
            canEdit = false;
            stateGenerator.stateToEdit = null;
            stateGenerator.status = 0;
        }
    }

    public void OpenEditStateOptions() //THIS EVENT IS TRIGGERED BY THE EDIT STATE BUTTON
    {
        if(checks.canModify)
        {
            stateGenerator.ShowPopUpWindow();
            stateGenerator.status = 1; 
            stateGenerator.stateToEdit = gameObject;
            EditStateOptions.SetActive(false);
        }
    }
}
