using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemoveConditions : MonoBehaviour
{
    private TMP_Text conditionText;
    private bool hover = false;  
    private Vector3 mousePos; 
    [HideInInspector] public bool selected = false; 

    private void Awake()
    {
        conditionText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        UnityEngine.Debug.Log("Hovering: " + conditionText.text + " -> " + hover);

        if(selected)
        {
            conditionText.color = new Color32(255, 0, 0, 255);
            if(Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
            {
                GameObject.Find("ScriptHolder").GetComponent<ConditionSetter>().RemoveCondition(conditionText.text);
                Destroy(gameObject);
            }
        }
        else conditionText.color = new Color32(0, 0, 0, 255);

        if(hover)
        {
            if(Input.GetMouseButtonDown(0))
            {
                selected = !selected; 
                
                //set selected to false for all the other conditions
                foreach(Transform condition in transform.parent)
                {
                    if(condition.gameObject != gameObject) condition.gameObject.GetComponent<RemoveConditions>().selected = false; 
                }
            }
        }
    }

    private void LateUpdate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; 
    }

    public void OnMouseEnter()
    {
        hover = true; 
    }
    public void OnMouseExit()
    {
        hover = false; 
    }
}
