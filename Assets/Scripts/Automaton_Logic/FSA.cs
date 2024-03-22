using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FSA : MonoBehaviour
{
    private Button runBtn; //button to run automaton
    private Button closeBtn; //button to close console
    private int i=0; //counter for errors

    [Header("Script References")]
    private EntryTransition entryTransition; //entry transition script

    [Header("Console Logs")]
    private GameObject consoleMenu, console, conErr, conSuc; //console || console area || console error || console success
    private bool consoleOpen = false; //is the console open?

    [Header("General References")]
    private GameObject stateHolder; //gameobject that holds all the states

    private void Awake()
    {
        consoleMenu = GameObject.Find("ConsoleLog");
        console = consoleMenu.transform.Find("ScrollArea").transform.Find("Content").gameObject;
        conErr = Resources.Load<GameObject>("Prefabs/UI_Elements/ConsoleError");
        conSuc = Resources.Load<GameObject>("Prefabs/UI_Elements/ConsoleSuccess");

        entryTransition = GameObject.Find("EntryState").GetComponent<EntryTransition>();

        runBtn = GetComponent<Button>();
        runBtn.onClick.AddListener(OpenConsole);
        closeBtn = consoleMenu.transform.Find("CloseConsole").GetComponent<Button>();
        closeBtn.onClick.AddListener(CloseConsole);

        stateHolder = GameObject.Find("State_Holder");

        consoleMenu.SetActive(false);
    }

    private void Update()
    {

    }

    private void OpenConsole()
    {
        if(!consoleOpen)
        {
            consoleMenu.SetActive(true);
            consoleOpen = true;
            if(stateHolder.transform.childCount == 1) PrintError("No states were created");
            if(!entryTransition.isSet) PrintError("Entry State not set");
        }
    }

    private void CloseConsole()
    {
        consoleMenu.SetActive(false);
        consoleOpen = false;
        //delete all logs
        foreach (Transform child in console.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void PrintError(string message)
    {
        GameObject error = Instantiate(conErr, console.transform);
        error.transform.Rotate(0, 0, 0);
        error.transform.SetParent(console.transform);
        error.GetComponent<TMP_Text>().text = message;
        i++; //increment the error counter
    }

    private void PrintSuccess(string message)
    {
        GameObject success = Instantiate(conSuc, console.transform);
        success.transform.Rotate(0, 0, 0);
        success.transform.SetParent(console.transform);
        success.GetComponent<TMP_Text>().text = message;
    }

    private void CheckTransitions()
    {
        foreach(Transform state in stateHolder.transform)
        {
            if(state.name != "EntryState")
            {
                //check if on a single state more transitions have the same condition
                
            }
        }
    }
}
