using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPointer : MonoBehaviour
{
    private Vector3 mousePos;
    private StateGenerator stateGenerator;

    private void Awake()
    {
        stateGenerator = GetComponent<StateGenerator>();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void LateUpdate()
    {
        if(!stateGenerator.menuShown) transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
}
