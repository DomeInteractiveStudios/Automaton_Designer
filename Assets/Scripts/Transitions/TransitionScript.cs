using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionScript : MonoBehaviour
{
    public GameObject[] states = new GameObject[3]; // 0 = start, 1 = middle, 2 = end
    [HideInInspector] public float midPointOffset; //offset of the middle point from the start and end points
    public string[] conditions; //list of conditions to be met for the state to follow this transition
    [HideInInspector] public bool isAuto = false, goUnder = false; //check if the transition is an auto transition || check if the middle point should go under the states
    private GameObject condPopUp, stateHolder, conditionText; //condition pop up window || state holder || condition text to add to the condition list scroll area
    private LineRenderer lineRenderer; //transition line
    private bool belowMinum = false; //check if the middle point is below the minimum offset
    private float vertexCount = 12f; //number of vertices in the line renderer
    private TMP_InputField conditionInput; //condition input field
    [SerializeField] private TMP_Text conditionVisual; //conditions text above transition line
    private Button addConditionButton, submitButton; //add condition button
    private List<string> conList = new List<string>(); //list of conditions
    private List<GameObject> transList = new List<GameObject>(); //list of transitions

    private void Awake()
    {   
        lineRenderer = GetComponent<LineRenderer>();
        condPopUp = GameObject.Find("FixedUI").transform.Find("Canvas").transform.Find("CondPopUp-Window").gameObject;
        condPopUp.SetActive(false);
        stateHolder = GameObject.Find("StateHolder");
        conditionText = Resources.Load<GameObject>("Prefabs/UI_Elements/ConditionText");

        conditionInput = condPopUp.transform.Find("InputCondition").GetComponent<TMP_InputField>();
        addConditionButton = condPopUp.transform.Find("Add Condition").GetComponent<Button>();
        submitButton = condPopUp.transform.Find("Submit").GetComponent<Button>();

        addConditionButton.onClick.AddListener(AddCondition);
        submitButton.onClick.AddListener(CloseConditionMenu);
    }

    private void Update()
    {
        /*CHECKS*/
        if(states[0].transform.position.x < states[2].transform.position.x) goUnder = true;
        else goUnder = false;
        /*CHECKS*/
    }

    private void LateUpdate()
    {
        if(!isAuto)
        {
            if(goUnder) states[1].transform.position = new Vector3((states[0].transform.position.x + states[2].transform.position.x) / 2, (states[0].transform.position.y + states[2].transform.position.y) - midPointOffset, 0);
            else states[1].transform.position = new Vector3((states[0].transform.position.x + states[2].transform.position.x) / 2, (states[0].transform.position.y + states[2].transform.position.y) + midPointOffset, 0);
        }

        var pointList = new List<Vector3>();
        for(float ratio = 0; ratio <= 1; ratio += 1/vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(states[0].transform.position, states[1].transform.position, ratio);
            var tangentLineVertex2 = Vector3.Lerp(states[1].transform.position, states[2].transform.position, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());

        /*WRITE CONDITIONS ABOVE THE TRANSITION LINE RENDERER*/
        if(conditions.Length != 0)
        {
            string condText = "";
            for(int i = 0; i < conditions.Length; i++)
            {
                if(i != conditions.Length - 1) condText += conditions[i] + ", ";
                else condText += conditions[i];
            }

            conditionVisual.text = condText;
        }
    }

    public void OpenConditionMenu()
    {
        condPopUp.SetActive(true);
        stateHolder.SetActive(false);
    }

    private void CloseConditionMenu()
    {
        condPopUp.SetActive(false);
        stateHolder.SetActive(true);

        /*COPY EVERY CONDTION FROM THE LIST TO THE ARRAY*/
        conditions = new string[conList.Count];
        for(int i = 0; i < conList.Count; i++)
        {
            conditions[i] = conList[i];
        }

        /*WHEN CLOSING THE MENU YOU SHOULD DESTROY EVERY CONTENT OF CHILD, AND RE-ADD THEM LATER WHEN OPENING THE POP UP THROUGH THE EDIT SCREEN*/
        foreach(Transform child in condPopUp.transform.Find("ConditionList").transform.Find("ScrollArea").transform.Find("Content"))
        {
            Destroy(child.gameObject);
        }
    }

    public void AddCondition()
    {
        GameObject content = condPopUp.transform.Find("ConditionList").transform.Find("ScrollArea").transform.Find("Content").gameObject;
        GameObject newCondition = Instantiate(conditionText, content.transform.position, content.transform.rotation);
        newCondition.transform.Rotate(0, 0, 0);
        newCondition.transform.localScale = newCondition.transform.localScale/50;
        newCondition.transform.SetParent(content.transform);
        if(conditionInput.text != "")   
        {
            conList.Add(conditionInput.text); //add the condition to the list of conditions
            newCondition.GetComponent<TMP_Text>().text = conditionInput.text;
        }
        conditionInput.text = "";
    }

    public void DestroyTransition()
    {
        Destroy(gameObject);
    }
}
