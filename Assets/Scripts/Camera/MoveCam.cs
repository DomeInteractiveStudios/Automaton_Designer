using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        //move camera on mouse drag
        if (((Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt)) || Input.GetMouseButton(2)) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            transform.position += new Vector3(Input.GetAxis("Mouse X") * 0.5f, -Input.GetAxis("Mouse Y") * 0.5f, 0);
        }

        //zoom in and out
        if (Input.GetAxis("Mouse ScrollWheel") > 0f  && cam.orthographicSize >= 5.5f) cam.orthographicSize -= 0.5f;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && cam.orthographicSize <= 9.5f) cam.orthographicSize += 0.5f;
    }
}
