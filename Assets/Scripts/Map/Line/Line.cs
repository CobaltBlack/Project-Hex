using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour
{
    LineRenderer myLine;

	void Awake ()
    {
        myLine = GetComponent<LineRenderer>();
        myLine.SetVertexCount(2); // amount of edges (2 for a straight line)
	}

    public void DrawLine(Transform origin, Transform target)
    {
        myLine.SetPosition(0, origin.position);
        myLine.SetPosition(1, target.position);

        float distance = Vector3.Distance(origin.position, target.position);
        myLine.GetComponent<Renderer>().material.mainTextureScale = new Vector2(distance * 2, 1);

        StartCoroutine(ChangeAlpha(myLine, 1f));
    }

    IEnumerator ChangeAlpha(LineRenderer target, float time)
    {
        Renderer renderer = target.GetComponent<Renderer>();

        float originalStartAlpha = 0.5f;
        float destinationStartAlpha = 1f;

        float originalEndAlpha = 0f;
        float destinationEndAlpha = 1f;

        float currentTime = 0.0f;

        Color startColor = Color.black;
        Color endColor = Color.black;

        do
        {
            startColor.a = Mathf.Lerp(originalStartAlpha, destinationStartAlpha, currentTime / time);
            endColor.a = Mathf.Lerp(originalEndAlpha, destinationEndAlpha, currentTime / time);

            target.SetColors(startColor, endColor);
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= time);
    }

    public void EraseLine()
    {
        StartCoroutine(EraseAlpha(myLine, 1f));
    }

    IEnumerator EraseAlpha(LineRenderer target, float time)
    {
        Renderer renderer = target.GetComponent<Renderer>();

        float originalStartAlpha = 1f;
        float destinationStartAlpha = 0f;

        float originalEndAlpha = 1f;
        float destinationEndAlpha = 0f;

        float currentTime = 0.0f;

        Color startColor = Color.black;
        Color endColor = Color.black;

        do
        {
            startColor.a = Mathf.Lerp(originalStartAlpha, destinationStartAlpha, currentTime / time);
            endColor.a = Mathf.Lerp(originalEndAlpha, destinationEndAlpha, currentTime / time);

            target.SetColors(startColor, endColor);
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= time);

        // destroy self after fading away
        Destroy(gameObject);
    }
}
