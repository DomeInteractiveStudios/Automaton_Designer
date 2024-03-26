using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingScript : MonoBehaviour
{
    [SerializeField] private Checks checks; // Reference to the Checks scriptable object.

    private void Awake()
    {
        checks.canModify = true;
    }
}
