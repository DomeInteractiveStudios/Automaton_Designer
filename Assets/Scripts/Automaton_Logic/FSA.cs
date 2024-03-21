using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FSA : MonoBehaviour
{
    private Button btn; //button to run automaton
    private int i=0; //counter for errors

    [Header("Console Logs")]
    private GameObject consoleMenu, console, conErr, conSuc; //console || console area || console error || console success

    [Header("General References")]
    private GameObject stateHolder; //gameobject that holds all the states

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OpenConsole);

        consoleMenu = GameObject.Find("ConsoleLog");
        console = consoleMenu.transform.Find("ScrollArea").transform.Find("Content").gameObject;
        conErr = Resources.Load<GameObject>("Prefabs/UI_Elements/ConsoleError");
        conSuc = Resources.Load<GameObject>("Prefabs/UI_Elements/ConsoleSuccess");

        stateHolder = GameObject.Find("State_Holder");

        consoleMenu.SetActive(false);
    }

    private void Update()
    {
        if(stateHolder.transform.childCount == 1) PrintError("No states were created");
    }

    private void OpenConsole()
    {
        consoleMenu.SetActive(true);
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
}
