using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    Vector2 lastMousePos;

    public int minZoom = 1;
    public int maxZoom = 5;

    void Update()
    {
        // Mouse Zoom
        GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize * (1f - Input.GetAxis("Mouse ScrollWheel")), minZoom, maxZoom);

        // Mouse Pan
        Vector2 currMousePos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            // Mouse was clicked this frame, so not dragging yet.
        }
        else if (Input.GetMouseButton(1))
        {
            // Mouse button still down -- let's move the camera!
            transform.Translate(lastMousePos - currMousePos);
            currMousePos = GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        }

        lastMousePos = currMousePos;
    }
}
