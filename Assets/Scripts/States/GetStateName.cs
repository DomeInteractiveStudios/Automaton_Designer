using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;   // Required when Using UI elements.


public class GetStateName : MonoBehaviour
{
    private StateScript stateScript; // Reference to the StateScript component.
    private TMP_Text proText; // Reference to the TextMesh component.

    private Transform parent; // Reference to the Transform component.
    private Transform grandParent; // Reference to the Transform component.
    private Transform grandGrandParent; // Reference to the Transform component.
    private Transform stateParent; // Reference to the Transform component.

    private void Awake()
    {
        parent = transform.parent; // Get the parent of the GameObject.
        grandParent = parent.parent; // Get the parent of the parent of the GameObject.
        grandGrandParent = grandParent.parent; // Get the parent of the parent of the parent of the GameObject.
        stateParent = grandGrandParent.parent; // Get the parent of the parent of the parent of the parent of the GameObject.
        stateScript = stateParent.GetComponent<StateScript>(); // Get the StateScript component from the GameObject.
        proText = GetComponentInChildren<TextMeshProUGUI>(); // Get the TextMesh component from the child object.
    }

    private void Update()
    {
        proText.text = stateScript.state.name; // Set the text to the name of the state.
    }
}
