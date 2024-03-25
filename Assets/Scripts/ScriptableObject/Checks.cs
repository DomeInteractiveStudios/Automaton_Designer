using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Checks", menuName = "ScriptableObject/Checks")]
public class Checks : ScriptableObject
{
    public bool canModify; // Boolean to check if the state can be modified.
}
