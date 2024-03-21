using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateGenerator : MonoBehaviour
{
    private StateScript statePrefabScript;
    private GameObject creationMenu;
    private GameObject statePrefab;
    private GameObject popUpWindow;
    private Vector3 lastMousePos; //last mouse position before opening the pop up window
    [SerializeField] private TMP_InputField stateName; //STATE NAME INPUT FIELD
    [SerializeField] private TMP_Dropdown stateType; //0 = regular, 1 = final
    [SerializeField] private TMP_Text submitText; //0 = Create, 1 = Edit
    [HideInInspector] public bool menuShown = false; //THIS IS TO CHECK IF THE CREATE A NEW STATE BUTTON IS SHOWN OR NOT
    [HideInInspector] public bool canCreate = true; //THIS IS TO CHECK IF THE CREATE A NEW STATE BUTTON CAN BE SHOWN OR NOT
    [HideInInspector] public int status; //THIS IS TO CHECK IF THE STATE IS TO BE CREATED OR EDITED
    [HideInInspector] public GameObject stateToEdit; //THIS IS TO CHECK WHICH STATE IS TO BE EDITED
    [HideInInspector] public GameObject stateHolder; //GAMEOBJECT THAT HOLDS ALL THE STATES
    private int i=1; //counts the number of states with no name created
    private Vector3 menuPos; //position of the creation menu
    private float saveSize; //save the size of the camera

    private void Awake()
    {
        statePrefab = Resources.Load<GameObject>("Prefabs/State");
        creationMenu = transform.GetChild(0).Find("CreationMenu").gameObject;
        popUpWindow = GameObject.Find("FixedUI").transform.Find("Canvas").transform.Find("PopUp-Window").gameObject;
        stateHolder = GameObject.Find("State_Holder");
        creationMenu.SetActive(false);
        popUpWindow.SetActive(false);
    }
    private void Update()
    {
        popUpWindow.transform.position = menuPos; //make the menu follow the camera

        if (Input.GetKeyDown(KeyCode.Mouse1) && canCreate && !menuShown && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) //SHOW THE CREATE A NEW STATE BUTTON ON MOUSE CLICK
        {
            ShowCreationMenu(); 
        }
        else if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse2)) && menuShown) //HIDE THE CREATE A NEW STATE BUTTON ON MOUSE CLICK
             {
                 HideCreationMenu();
             }

        /*SUBMIT BUTTON TEXT*/
        if(status == 0) submitText.text = "Create State";
        else if(status == 1) submitText.text = "Edit State";
        /*SUBMIT BUTTON TEXT*/
    }

    private void LateUpdate()
    {
        menuPos = Camera.main.transform.position;
        menuPos.z = 0;
    }

    public void GenerateState() //THIS EVENT IS TRIGGERED BY THE CREATE A NEW STATE BUTTON
    {
        ShowPopUpWindow(); 
        HideCreationMenu();
    }

    private void ShowCreationMenu() //SHOWS THE CREATE A NEW STATE BUTTON 
    {
        creationMenu.SetActive(true);
        saveSize = Camera.main.orthographicSize;
        lastMousePos = creationMenu.transform.position;
        menuShown = true;
    }

    private void HideCreationMenu() //HIDES THE CREATE A NEW STATE BUTTON
    {
        creationMenu.SetActive(false);
        menuShown = false;
    }

    public void ShowPopUpWindow() //SHOW STATE CREATION WINDOW
    {
        popUpWindow.SetActive(true);
        Camera.main.orthographicSize = 5f;
        stateHolder.SetActive(false);
    }   

    private void HidePopUpWindow() //SHOW STATE CREATION WINDOW
    {
        popUpWindow.SetActive(false);
        Camera.main.orthographicSize = saveSize;
        stateHolder.SetActive(true);
    }

    public void StateHandeler() //HANDLE WHETHER TO CREATE OR TO EDIT STATE
    {
        switch(status)
        {
            case 0: 
                CreateState();
                break;
            case 1:
                EditState(stateToEdit);
                break;
            default:
                break;
        }
    }
    private void CreateState() //CREATES A NEW STATE THAT DOES NOT EXIST
    {
        GameObject newState = Instantiate(statePrefab, lastMousePos, Quaternion.identity);
        statePrefabScript = newState.GetComponent<StateScript>();
        newState.transform.SetParent(stateHolder.transform);
        if(stateName.text == "" || stateName.text == "State") //custom name for nameless states to be added later
        {
            if(i!=1)
            {
                statePrefabScript.state.name = $"State {i}";
            }
            else statePrefabScript.state.name = "State";

            i++;
        }
        else statePrefabScript.state.name = stateName.text;
        statePrefabScript.state.type = stateType.value == 0 ? StateScript.StateType.regular : StateScript.StateType.final;
        HidePopUpWindow();
    }

    private void EditState(GameObject stateToEdit) //EDITS AN EXISTING STATE
    {
        StateScript editScript = stateToEdit.GetComponent<StateScript>();
        if(stateName.text == "" || stateName.text == "State") //custom name for nameless states to be added later
        {
            if(i!=1)
            {
                statePrefabScript.state.name = $"State {i}";
            }
            else statePrefabScript.state.name = "State";
        }
        else editScript.state.name = stateName.text;
        editScript.state.type = stateType.value == 0 ? StateScript.StateType.regular : StateScript.StateType.final;
        HidePopUpWindow();
    }
}
