using UnityEngine;
using System.Collections;

public class GLLine : MonoBehaviour
{
    public Vector2 start;
    public Vector2 end;
    public int width;

    public Texture2D point;

    void Update()
    {
        DrawLine(start, end, width);
    }

    private void DrawLine(Vector2 start, Vector2 end, int width)
    {
        Vector2 d = end - start;
        float a = Mathf.Rad2Deg * Mathf.Atan(d.y / d.x);
        if (d.x < 0)
            a += 180;

        int width2 = (int)Mathf.Ceil(width / 2);

        GUIUtility.RotateAroundPivot(a, start);
        GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), point);
        GUIUtility.RotateAroundPivot(-a, start);
    }
}