using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour
{
    public Vector2 start = Vector2.zero;
    public Vector2 stop = Vector2.zero;

    private Transform topCap;
    private Transform center;
    private Transform bottomCap;

    private float topCapLength;
    private float bottomCapLength;

    private float invCenterLength;

    private float minLength;

    void Start()
    {
        topCap = transform.FindChild("TopCap");
        center = transform.FindChild("Center");
        bottomCap = transform.FindChild("BottomCap");

        topCapLength = 2 * topCap.GetComponent<Renderer>().bounds.extents.x;
        invCenterLength = 1 / (2 * center.GetComponent<Renderer>().bounds.extents.x);
        bottomCapLength = 2 * bottomCap.GetComponent<Renderer>().bounds.extents.x;

        minLength = topCapLength + bottomCapLength;
    }

    void Update()
    {
        // For testing purposes
        if (Input.GetButton("Fire1"))
        {
            stop = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Draw(start, stop);
    }

    void Draw(Vector2 start, Vector2 stop)
    {
        Vector2 dir = stop - start;

        float length = dir.magnitude;

        transform.position = start;

        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        if (length <= minLength)
        {
            bottomCap.localPosition = minLength * Vector2.right;

            center.localScale = new Vector3(1, 0, 1);

            transform.localScale = new Vector3(length / minLength, 1, 1);
        }
        else
        {
            bottomCap.localPosition = length * Vector2.right;

            length -= minLength;

            center.localScale = new Vector3(1, length * invCenterLength, 1);

            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}