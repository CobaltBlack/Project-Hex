using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour
{
    LineRenderer myLine;
    Transform target;

	// Use this for initialization
	void Awake ()
    {
        //target = GameObject.Find("Target").transform;
        myLine = GetComponent<LineRenderer>();
        myLine.SetVertexCount(2); // amount of edges (2 for a straight line)
	}
	
    /*
	// Update is called once per frame
	void Update ()
    {
        myLine.SetPosition(0, gameObject.transform.position);
        myLine.SetPosition(1, target.position);

        float distance = Vector3.Distance(gameObject.transform.position, target.position);
        myLine.GetComponent<Renderer>().material.mainTextureScale = new Vector2(distance * 2, 1);
	}
    */

    public void DrawLine(Transform origin, Transform target)
    {
        myLine.SetPosition(0, origin.position);
        myLine.SetPosition(1, target.position);

        float distance = Vector3.Distance(origin.position, target.position);
        myLine.GetComponent<Renderer>().material.mainTextureScale = new Vector2(distance * 2, 1);
    }
}
