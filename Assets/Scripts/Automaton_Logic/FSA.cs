using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class FSA : MonoBehaviour
{
    private Button runBtn; //button to run automaton
    private Button closeBtn; //button to close console
    private int i=0; //counter for errors

    [Header("Script References")]
    private EntryTransition entryTransition; //entry transition script
    [SerializeField] private Checks checks; //checks scriptable object

    [Header("Console Logs")]
    private GameObject consoleMenu, console, conErr, conSuc, executeButton; //console || console area || console error message || console success message || execute automaton button
    private bool consoleOpen = false; //is the console open?

    [Header("General References")]
    private GameObject stateHolder; //gameobject that holds all the states
    private List <string> conditions = new List<string>(); //list of all conditions on a state

    private void Awake()
    {
        consoleMenu = GameObject.Find("ConsoleLog");
        console = consoleMenu.transform.Find("ScrollArea").transform.Find("Content").gameObject;
        conErr = Resources.Load<GameObject>("Prefabs/UI_Elements/ConsoleError");
        conSuc = Resources.Load<GameObject>("Prefabs/UI_Elements/ConsoleSuccess");
        executeButton = Resources.Load<GameObject>("Prefabs/UI_Elements/Execute");

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
            CheckTransitions();
            ExecuteButton();
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
        i=0; //reset the error counter
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
                conditions.Clear();
                for(int k=0; k<state.childCount; k++)
                {
                    Transform child = state.GetChild(k);
                    if(child.name.Contains("Transition"))
                    {
                        //create a list of all conditions in all the transitions
                        for(int j=0; j<child.GetComponent<TransitionScript>().conditions.Length; j++) conditions.Add(child.GetComponent<TransitionScript>().conditions[j]);    
                    }
                }
                //check if there are any duplicates
                if(conditions.Count != conditions.Distinct().Count())
                {
                    PrintError("State ''" + state.GetComponent<StateScript>().state.name + "'' has duplicate conditions");
                }
                else
                {
                    PrintSuccess("State ''" + state.GetComponent<StateScript>().state.name + "'' has no duplicate conditions");
                }
            }
        }
    }

    private void ExecuteButton()
    {
        //execute the automaton
        if(i==0)
        {
            //no errors
            PrintSuccess("Automaton can be executed");

            //show execute button
            GameObject execBtn = Instantiate(executeButton, console.transform);
            execBtn.transform.Rotate(0, 0, 0);
            execBtn.transform.SetParent(console.transform);
            execBtn.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(ExecuteAutomaton);
        }
    }

    private void ExecuteAutomaton()
    {
        //execute the automaton
        CloseConsole();
        //freeze input util execution is done
        checks.canModify = false;
    }   

    private void EndExecution()
    {
        //unfreeze input
        checks.canModify = true;
    }
}
